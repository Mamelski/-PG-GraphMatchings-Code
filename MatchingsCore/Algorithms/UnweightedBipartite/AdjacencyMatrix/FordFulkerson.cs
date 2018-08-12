namespace MatchingsCore.Algorithms.UnweightedBipartite
{
    using System.Linq;

    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The ford_ fulkerson.
    /// </summary>
    public partial class Ford_Fulkerson
    {
        /// <summary>
        /// The source id.
        /// </summary>
        private int sourceIndex;

        /// <summary>
        /// The sink id.
        /// </summary>
        private int sinkIndex;

        private AdjacencyMatrixGraph graph;
        
        /// <summary>
        /// The get maximum matching.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        public void GetMaximumMatching(AdjacencyMatrixGraph graph)
        {
            this.graph = graph;

            this.sourceIndex = this.graph.AddNodeAtTheEndAndReturnId();
            this.sinkIndex = this.graph.AddNodeAtTheEndAndReturnId();

            this.AddEdgesToSourceAndSink(ref graph);

            int a = 0;
            //this.RunEdmondsKarpsAlgorithm(ref graph);
        }

        /// <summary>
        /// The add edges to source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void AddEdgesToSourceAndSink(ref AdjacencyMatrixGraph graph)
        {
            for (var i = 0; i < graph.Nodes.Count - 2; ++i)
            {
                // Add edge to source Source
                if (graph.Nodes[i].Color == 0)
                {
                    graph.Matrix[this.sourceIndex][i] = 1;
                }

                // Add edge to sink
                if (graph.Nodes[i].Color == 1)
                {
                    graph.Matrix[i][this.sourceIndex] = 1;
                }
            }
        }

        ///// <summary>
        ///// The run edmonds karp algorithm.
        ///// </summary>
        ///// <param name="graph">
        ///// The graph.
        ///// </param>
        //private void RunEdmondsKarpsAlgorithm(ref AdjacencyListGraph graph)
        //{
        //}

        //bool CanNodeCanBeAddedToMatching(Node node, AdjacencyListGraph graph,
        //         bool[] seen, int[] matchR)
        //{

        //    // Try every job one by one
        //    for (int v = 0; v < N; v++)
        //    {
        //        // If applicant node is interested 
        //        // in job v and v is not visited
        //        if (graph[node.Id].Edges.Any(e => e.Destination.Id == v) && !seen[v])
        //        {
        //            // Mark v as visited
        //            seen[v] = true;

        //            // If job 'v' is not assigned to
        //            // an applicant OR previously assigned 
        //            // applicant for job v (which is matchR[v])
        //            // has an alternate job available.
        //            // Since v is marked as visited in the above 
        //            // line, matchR[v] in the following recursive 
        //            // call will not get job 'v' again
        //            if (matchR[v] < 0 || bpm(bpGraph, matchR[v],
        //                    seen, matchR))
        //            {
        //                matchR[v] = node;
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}




    }
}
