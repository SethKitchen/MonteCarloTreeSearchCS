namespace MonteCarloTreeSearch
{
    /// <summary>
    /// For use in a MonteCarloNode. Add onto this with your unique data.
    /// </summary>
    public class BasicNodeInfo
    {
        /// <summary>
        /// Normally this is a Game State for whatever game you are playing.
        /// </summary>
        public IState NodeState { get; set; }

        /// <summary>
        /// Normally this is -1 for Player 1 and 1 for Player 2
        /// </summary>
        public int Turn { get; set; }

        /// <summary>
        /// A unique ID we can use for hashing in a Dictionary
        /// </summary>
        public string Id { get; set; }
    }
}