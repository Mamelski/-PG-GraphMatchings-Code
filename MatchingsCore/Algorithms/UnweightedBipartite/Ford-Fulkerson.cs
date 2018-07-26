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
        private int sourceId;

        /// <summary>
        /// The sink id.
        /// </summary>
        private int sinkId;
        
        /// <summary>
        /// The get maximum matching.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        public void GetMaximumMatching(Graph graph)
        {
            // We are counting from zero, so last index in original graph is graph.Size -1
            this.sourceId = graph.Size;
            this.sinkId = graph.Size + 1;

            this.AddSourceAndSink(ref graph);
            this.AddEdgesToSourceAndSink(ref graph);
        }

        /// <summary>
        /// The add edges to source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private void AddEdgesToSourceAndSink(ref Graph graph)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The add source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void AddSourceAndSink(ref Graph graph)
        {
            var sourceNode = new Node { Id = this.sourceId };
            graph.AddNode(sourceNode);

            var sinkNode = new Node { Id = this.sinkId };
            graph.AddNode(sinkNode);
        }
    }
}
