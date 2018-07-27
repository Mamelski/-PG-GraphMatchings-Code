namespace MatchingsCore.Algorithms.UnweightedBipartite
{
    using System.Linq;

    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The ford_ fulkerson.
    /// </summary>
    public class Ford_Fulkerson
    {
        /// <summary>
        /// The source id.
        /// </summary>
        private int sourceIndex;

        /// <summary>
        /// The sink id.
        /// </summary>
        private int sinkIndex;
        
        /// <summary>
        /// The get maximum matching.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        public void GetMaximumMatching(Graph graph)
        {
            var firstFreeIndex = graph.Size;

            // We are counting from zero, so last index in original graph is graph.Size -1
            this.sourceIndex = firstFreeIndex;
            this.sinkIndex = firstFreeIndex + 1;

            this.AddSourceAndSink(ref graph);
            this.AddEdgesToSourceAndSink(ref graph);

            this.RunEdmondsKarpsAlgorithm(ref graph);
        }

        /// <summary>
        /// The add source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void AddSourceAndSink(ref Graph graph)
        {
            var sourceNode = new Node { Id = this.sourceIndex };
            graph.AddNode(sourceNode);

            var sinkNode = new Node { Id = this.sinkIndex };
            graph.AddNode(sinkNode);
        }

        /// <summary>
        /// The add edges to source and sink.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void AddEdgesToSourceAndSink(ref Graph graph)
        {
            for (var i = 0; i < graph.Size - 2; ++i)
            {
                // Add edge to source Source
                if (graph[i].Color == 0)
                {
                    var edge = new Edge { Source = graph[this.sourceIndex], Destination = graph[i] };
                    graph[this.sourceIndex].Edges.Add(edge);

                    edge = new Edge { Source = graph[i], Destination = graph[this.sourceIndex] };
                    graph[i].Edges.Add(edge);
                }

                // Add edge to sink
                if (graph[i].Color == 1)
                {
                    var edge = new Edge { Source = graph[this.sinkIndex], Destination = graph[i] };
                    graph[this.sinkIndex].Edges.Add(edge);

                    edge = new Edge { Source = graph[i], Destination = graph[this.sinkIndex] };
                    graph[i].Edges.Add(edge);
                }
            }
        }

        /// <summary>
        /// The run edmonds karp algorithm.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void RunEdmondsKarpsAlgorithm(ref Graph graph)
        {
        }

        bool bpm(Graph graph , Node node,
                 bool[] seen, int[] matchR)
        {
            // Try every job one by one
            for (int v = 0; v < N; v++)
            {
                // If applicant node is interested 
                // in job v and v is not visited
                if (graph[node.Id].Edges.Any(e => e.Destination.Id == v) && !seen[v])
                {
                    // Mark v as visited
                    seen[v] = true;

                    // If job 'v' is not assigned to
                    // an applicant OR previously assigned 
                    // applicant for job v (which is matchR[v])
                    // has an alternate job available.
                    // Since v is marked as visited in the above 
                    // line, matchR[v] in the following recursive 
                    // call will not get job 'v' again
                    if (matchR[v] < 0 || bpm(bpGraph, matchR[v],
                            seen, matchR))
                    {
                        matchR[v] = node;
                        return true;
                    }
                }
            }
            return false;
        }

       

       
    }
}
