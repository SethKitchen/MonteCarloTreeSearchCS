using System.Collections.Generic;

namespace MonteCarloTreeSearch
{
    /// <summary>
    /// Abstract class implementing the basis of the
    /// <a href="http://en.wikipedia.org/wiki/Monte-Carlo_tree_search">Monte Carlo Tree Search</a> algorithm :
    /// <ol>
    /// <li>selection</li>
    /// <li>expansion</li>
    /// <li>simulation</li>
    /// <li>back propagation</li>
    /// </ol>
    /// The is a stateful implementation of the algorithm meaning that each of the four above steps modify
    /// the state of the game and take care of restoring it before returning... To use this algorithm, just
    /// implement all of the abstract methods of this class (and respect the contract of each one). Then,
    /// get the best choice for the current player with <seealso cref="getBestTransition()"/> and do it by calling
    /// <seealso cref="doTransition(Transition)"/>. <seealso cref="undoTransition(Transition)"/> allow you to rollback a choice.
    /// <br/>
    /// The state SHOULD be stored in this class. <seealso cref="Node"/> SHOULD only be used to store necessary information
    /// related to number of simulations and associated wins/loose ratio...
    /// </summary>
    /// @param <T> a <seealso cref="Transition"/> representing an atomic action that modifies the state
    /// 
    /// @author antoine vianey </param>
    // TODO keep track of number of nodes to evaluate memory footprint
    public abstract class MonteCarloTreeSearch<T> where T : ITransition
    {

        /// <summary>
        /// This is where we are.
        /// Each <seealso cref="Node"/> keeps a reference to its parent <seealso cref="Node"/> and to each child <seealso cref="Node"/>.
        /// </summary>
        private Node<T> current;

        public MonteCarloTreeSearch()
        {
            Reset();
        }

        /// <summary>
        /// Creates a new exploration tree.
        /// </summary>
        public virtual void Reset()
        {
            current = new Node<T>(null, default(T), false);
        }

        /// <summary>
        /// Get the best <seealso cref="Transition"/> for the current player.
        /// Playing a <seealso cref="Transition"/> MUST be done by calling <seealso cref="doTransition(Transition)"/>
        /// unless next call to this method WILL rely on a wrong origin. </summary>
        /// <returns> the best <seealso cref="Transition"/> for the current player or null if the current player has no possible move. </returns>
        public virtual T BestTransition
        {
            get
            {
                if (PossibleTransitions.Count == 0)
                {
                    // no possible transition
                    // isOver MUST be true.
                    return default(T);
                }
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final int currentPlayer = getCurrentPlayer();
                int currentPlayer = CurrentPlayer;
                Node<T> nodeToExpand;
                bool stop = false;
                // TODO : do it in a interuptable Thread
                do
                {
                    nodeToExpand = Selection();
                    if (nodeToExpand == null)
                    {
                        break;
                    }
                    // the tree has not been fully explored yet
                    Node<T> expandedNode = Expansion(nodeToExpand);
                    int winner = Simulation();
                    BackPropagation(expandedNode, winner);
                } while (!stop);
                // state is restored
                T best = default(T);
                double bestValue = double.NegativeInfinity;
                // all possible transitions have been set on root node
                // see expansion(N node)
                foreach (var x in current.Childs)
                {
                    Node<T> child = x.Value;
                    double value = child.Ratio(currentPlayer);
                    if (value > bestValue)
                    {
                        bestValue = value;
                        best = child.Transition;
                    }
                }
                return best;
            }
        }

        /// <summary>
        /// Update the context and change the root of the tree to this context so that it reflects the
        /// realization of the given <seealso cref="Transition"/>. This method is the same as <seealso cref="makeTransition(Transition)"/>
        /// but it also change the root of the tree to the <seealso cref="Node"/> reached by the given <seealso cref="Transition"/>.
        /// MUST only be called with a <seealso cref="Transition"/> returned by <seealso cref="getBestTransition()"/>. </summary>
        /// <param name="transition"> The non null <seealso cref="Transition"/> to play </param>
        /// <seealso cref= #makeTransition(Transition) </seealso>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public final void doTransition(T transition)
        public void DoTransition(T transition)
        {
            MakeTransition(transition);
            current = current.GetChild(transition);
            current.MakeRoot();
        }

        /// <summary>
        /// Update the context and change the root of the tree to this context so that it reflects the <b>rollback</b> of the
        /// realization of the given <seealso cref="Transition"/>. This method is the same as <seealso cref="unmakeTransition(Transition)"/>
        /// but it also change the root of the tree to the origin <seealso cref="Node"/> of the given <seealso cref="Transition"/> in the tree. </summary>
        /// <seealso cref= #unmakeTransition(Transition) </seealso>
        public void UndoTransition(T transition)
        {
            UnmakeTransition(transition);
            current = new Node<T>(current);
        }

        // region MCTS

        /// <summary>
        /// Select a leaf <seealso cref="Node"/> to expand. The selection is done by calling <seealso cref="selectTransition(Node, int)"/>
        /// from child to child until we reach a leaf <seealso cref="Node"/>. The returned <seealso cref="Node"/> MIGHT be terminal (meaning
        /// it was an unexplored child of a leaf <seealso cref="Node"/>). </summary>
        /// <returns> The <seealso cref="Node"/> to expand or null if there's nothing else to expand... </returns>

        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") private Node<T> selection()
        private Node<T> Selection()
        {
            Node<T> n = current;
            Node<T> next;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int player = getCurrentPlayer();
            int player = CurrentPlayer;
            do
            {
                T transition = SelectTransition(n, player);
                if (transition == null)
                {
                    n.Terminal = true;
                    if (n == current)
                    {
                        return null;
                    }
                    else
                    {
                        // node has parent, rewind
                        UnmakeTransition(n.Transition);
                        next = n.Parent;
                    }
                }
                else
                {
                    next = n.GetChild(transition);
                    MakeTransition(transition);
                    if (next == null)
                    {
                        // this transition has never been explored
                        // create child node and expand it
                        next = new Node<T>(n, transition, Over);
                    }
                }
                n = next;
            } while (!n.IsLeaf());
            return n;
        }

        /// <summary>
        /// Expand the leaf <seealso cref="Node"/> by creating <strong>every</strong> child <seealso cref="Node"/>.<br/>
        /// The leaf <seealso cref="Node"/> to expand MIGHT be a terminal <seealso cref="Node"/>, as {<seealso cref="selection()"/>} MIGHT return a
        /// <seealso cref="Node"/> that was just created...
        /// After expansion, the leaf <seealso cref="Node"/> has all of its children created.<br/> </summary>
        /// <param name="leaf"> The leaf <seealso cref="Node"/> to expand.
        /// @return
        ///      The expanded <seealso cref="Node"/> to run the random simulation from.
        ///      The expanded <seealso cref="Node"/> MIGHT be a terminal <seealso cref="Node"/>. </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        //ORIGINAL LINE: private Node<T> expansion(final Node<T> leaf)
        private Node<T> Expansion(Node<T> leaf)
        {
            if (leaf.Terminal)
            {
                return leaf;
            }
            T transition = ExpansionTransition();
            if (transition != null)
            {
                // expand the path with the chosen transition
                MakeTransition(transition);
                return new Node<T>(leaf, transition, Over);
            }
            else
            {
                return leaf;
            }
        }

        /// <summary>
        /// Run a random simulation from the expanded position to get a winner. </summary>
        /// <returns> The winner designated by the random simulation. </returns>
        private int Simulation()
        {
            LinkedList<T> transitions = new LinkedList<T>();
            // do
            while (!Over)
            {
                T transition = SimulationTransition();
                MakeTransition(transition);
                transitions.AddLast(transition);
            }
            int winner = Winner;
            // undo
            while (transitions.Count > 0)
            {
                var last = transitions.Last.Value;
                transitions.RemoveLast();
                UnmakeTransition(last);
            }
            return winner;
        }

        /// <summary>
        /// Propagate the winner from the expanded <seealso cref="Node"/> up to the current root <seealso cref="Node"/> </summary>
        /// <param name="expandedNode"> The <seealso cref="Node"/> that was expanded. </param>
        /// <param name="winner"> The winner of the simulation. </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        //ORIGINAL LINE: private void backPropagation(Node<T> expandedNode, final int winner)
        private void BackPropagation(Node<T> expandedNode, int winner)
        {
            Node<T> n = expandedNode;
            while (n != null)
            {
                n.Result(winner);
                Node<T> parent = n.Parent;
                if (parent == null)
                {
                    // root reached
                    break;
                }
                UnmakeTransition(n.Transition);
                n = parent;
            }
        }

        // endregion

        // region API

        /// <summary>
        /// Method used to select a <seealso cref="Transition"/> to follow and reach a leaf <seealso cref="Node"/> to expand :
        /// <ul>
        /// <li>UCT : upper confident bound applied to trees</li>
        /// </ul>
        /// This method MUST NOT return a terminal <seealso cref="Node"/>. </summary>
        /// <param name="node"> a <seealso cref="Node"/> that has already been visited </param>
        /// <param name="player"> the player for which we are seeking a promising child <seealso cref="Node"/> </param>
        /// <returns> the next <seealso cref="Transition"/> to a non terminal <seealso cref="Node"/> in the selection step
        /// 		or null if there's no child to explore
        /// 		or null if there's only terminal child nodes </returns>
        /// <seealso cref= Node#isTerminal() </seealso>
        /// <seealso cref= UCT </seealso>
        public abstract T SelectTransition(Node<T> node, int player);

        /// <summary>
        /// Select the next <seealso cref="Transition"/> during the simulation step
        /// @return
        /// 		The next <seealso cref="Transition"/> in the simulation
        /// 		or null if {<seealso cref="getPossibleTransitions()"/> is null or empty
        /// </summary>
        public abstract T SimulationTransition();

        /// <summary>
        /// Choose the <seealso cref="Transition"/> to follow and expanded and the simulation from.
        /// @return
        ///      The desired <seealso cref="Transition"/> to get the expanded <seealso cref="Node"/> from
        /// 		or null if {<seealso cref="getPossibleTransitions()"/> is null or empty
        /// </summary>
        public abstract T ExpansionTransition();

        /// <summary>
        /// Update the context so it takes into account the realization of the given <seealso cref="Transition"/>.
        /// MUST only be called with a <seealso cref="Transition"/> returned by <seealso cref="getBestTransition()"/>. </summary>
        /// <param name="transition">
        /// 		A <seealso cref="Transition"/> returned by <seealso cref="getBestTransition()"/> or null </param>
        protected internal abstract void MakeTransition(T transition);

        /// <summary>
        /// Update the context so it takes into account the rollback of the given <seealso cref="Transition"/>.
        /// MUST only be called with the last <seealso cref="Transition"/> passed to <seealso cref="makeTransition(Transition)"/>. </summary>
        /// <param name="transition">
        /// 		A <seealso cref="Transition"/> returned by <seealso cref="getBestTransition()"/> or null </param>
        protected internal abstract void UnmakeTransition(T transition);

        /// <summary>
        /// Get possible transitions from the current position. Returned transitions MIGHT involves
        /// any of the players, taking into account actions such as pass, skip, fold, etc...
        /// @return
        ///      <seealso cref="System.Collections.Generic.ISet<object>"/> of possible transitions for the current position (including 'pass' or 'skip').
        ///      <seealso cref="System.Collections.Generic.ISet<object>"/> could be empty, in that case {<seealso cref="isOver()"/> MUST return true.
        /// </summary>
        public abstract ISet<T> PossibleTransitions { get; }

        /// <summary>
        /// MUST return true if there's no possible <seealso cref="Transition"/> from the current position </summary>
        /// <returns> true if <seealso cref="getPossibleTransitions()"/> returns an empty <seealso cref="System.Collections.Generic.ISet<object>"/> </returns>
        public abstract bool Over { get; }

        /// <summary>
        /// Return the index of the winner when <seealso cref="isOver()"/> returns true. </summary>
        /// <returns> the index of the winner </returns>
        // TODO : handle draw (also in node and backpropagation)
        public abstract int Winner { get; }

        /// <summary>
        /// Returns the index of the player for the current state.
        /// @return
        /// </summary>
        // TODO manage players internally
        public abstract int CurrentPlayer { get; }

        // endregion

    }
}