namespace MatchingsCore.Algorithms.UnweightedBipartite
{
    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The ford_ fulkerson.
    /// </summary>
    public class Ford_Fulkerson
    {
        /// <summary>
        /// The source id.
        /// </summary>
        private int sourceId = -1;

        /// <summary>
        /// The sink id.
        /// </summary>
        private int sinkId = -2;
        
        /// <summary>
        /// The get maximum matching.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        public void GetMaximumMatching(Graph graph)
        {
            this.AddSourceAndSink(graph);
        }

        /// <summary>
        /// The add source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void AddSourceAndSink(Graph graph)
        {
            throw new System.NotImplementedException();
        }
    }
}
