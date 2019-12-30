using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    //Helper class added by Java to C# Converter:

    //---------------------------------------------------------------------------------------------------------
    //	Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class is used to replace calls to some Java HashMap or Hashtable methods.
    //---------------------------------------------------------------------------------------------------------

    internal static class HashMapHelper
    {
        public static HashSet<KeyValuePair<TKey, TValue>> SetOfKeyValuePairs<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            HashSet<KeyValuePair<TKey, TValue>> entries = new HashSet<KeyValuePair<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
            {
                entries.Add(keyValuePair);
            }
            return entries;
        }

        public static TValue GetValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var success = dictionary.TryGetValue(key, out TValue ret);
            if(!success)
            {
                return default;
            }
            return ret;
        }
    }

    public class Node<T> where T : ITransition
    {

        public IDictionary<T, Node<T>> Childs { get; private set; }
        private readonly IDictionary<int, int> Wins; // TODO Arraylist
        public long Simulations { get; set; } = 0;

        public T Transition { get; }
        public Node<T> Parent { get; private set; }

        /// <summary>
        /// Create a child <seealso cref="Node"/>. </summary>
        /// <param name="parent"> The parent <seealso cref="Node"/> of the created node. </param>
        /// <param name="transition"> The transition from the parent <seealso cref="Node"/> to this node. </param>
        /// <param name="terminal"> Whether or not this <seealso cref="Node"/> is a terminal <seealso cref="Node"/>. </param>
        internal Node(Node<T> parent, T transition, bool terminal)
        {
            this.Terminal = terminal;
            this.Parent = parent;
            this.Transition = parent == null ? default : transition;
            this.Childs = new Dictionary<T, Node<T>>();
            this.Wins = new Dictionary<int, int>();
            if (parent != null)
            {
                parent.Childs[transition] = this;
            }
        }

        /// <summary>
        /// Create a parent <seealso cref="Node"/>. </summary>
        /// <param name="child"> </param>
        internal Node(Node<T> child)
        {
            this.Terminal = false;
            this.Parent = null;
            this.Transition = default;
            this.Childs = new Dictionary<T, Node<T>>();
            this.Wins = new Dictionary<int, int>();
            this.Simulations = child.Simulations;
            // copy stats
            Childs[child.Transition] = child;
            foreach (KeyValuePair<int, int> e in child.Wins.SetOfKeyValuePairs())
            {
                Wins[e.Key] = e.Value;
            }
        }

        /// <summary>
        /// A <seealso cref="Node"/> is terminal when there is no child to explore.
        /// The sub-Tree of this <seealso cref="Node"/> has been fully explored or the <seealso cref="Node"/> correspond
        /// to a configuration where <seealso cref="MonteCarloTreeSearch.isOver()"/> return true. </summary>
        /// <returns> true If the <seealso cref="Node"/> is a terminal <seealso cref="Node"/> </returns>
        // TODO propagate terminal information from node to node
        public virtual bool Terminal
        {
            get; set;
        }

        /// <summary>
        /// Get the value of the <seealso cref="Node"/> for the given player.
        /// The <seealso cref="Node"/> with the greater value will be picked
        /// as the best choice for this player. </summary>
        /// <param name="player">
        /// @return </param>
        public virtual double Value(int player)
        {
            return Wins[player];
        }


        ///
        /// Get the cild {@link Node} reach by the given {@link Transition}
        /// @param transition The {@link Transition}
        /// @return The child {@link Node} or null if there's no child known for the given {@link Transition}
        ///
        public Node<T> GetChild(T transition)
        {
            bool success = Childs.TryGetValue(transition, out Node<T> value);
            if(success)
            {
                return value;
            }
            return null;
        }

        ///
        /// Make this {@link Node} a root {@link Node} by removing the reference to its parent
        ///
        public void MakeRoot()
        {
            Parent = null;
        }

        /**
         * A leaf {@link Node} is a node with no child.
         * There's two case where a {@link Node} can be leaf :
         * <ol>
         * <li>The {@link Node} is a terminal {@link Node}</li>
         * <li>The {@link Node} has never been expanded (has no child)</li>
         * </ol>
         * @return
         * 		true if the {@link Node} is a leaf {@link Node}
         */
        public bool IsLeaf()
        {
            return Childs.Count == 0;
        }

        /**
         * Number of simulation back-propagated to this {@link Node} where the given player has won
         * @param player
         * @return
         */
        public double Ratio(int player)
        {
            bool success = Wins.TryGetValue(player, out int w);
            if (!success)
            {
                return 0;
            }
            else
            {
                return ((double)w) / Simulations;
            }
        }

        public long WinsF(int player)
        {
            bool success = Wins.TryGetValue(player, out int w);
            if (!success)
            {
                return 0;
            }
            else
            {
                return w;
            }
        }

        /**
         * Propagate the result of a simulation to this {@link Node}.
         * After a call to this method, {@link #simulations()} is incremented as well as
         * {@link #wins(int)} for the given winner.
         * @param winner The winner of the back-propagated simulation
         */
        public void Result(int winner)
        {
            Simulations++;
            bool success = Wins.TryGetValue(winner, out int w);
            if (!success)
            {
                Wins.Add(winner, 1);
            }
            else
            {
                Wins[winner] = w + 1;
            }
        }

        /**
         * Returns the {@link Collection} of all the child of this {@link Node}
         * @return
         * 		The return {@link Collection} MUST NOT be null
         * 		If the {@link Node} is the leaf {@link Node}, then an empty {@link Collection} is returned
         */
        public ICollection<Node<T>> GetChilds()
        {
            return Childs.Values;
        }

        /**
         * Get the child {@link Node} reach by the given {@link Transition}
         * @param transition The {@link Transition} to fetch the child {@link Node} from
         * @return
         */
        public Node<T> GetNode(T transition)
        {
            return Childs[transition];
        }
    }
}
