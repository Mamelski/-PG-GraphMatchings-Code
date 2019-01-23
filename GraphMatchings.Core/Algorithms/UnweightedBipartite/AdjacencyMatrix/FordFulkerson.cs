using GraphMatchings.Core.GraphRepresentation;

namespace MatchingsCore.Algorithms.UnweightedBipartite.AdjacencyMatrix
{
    using System.Collections.Generic;
    using System.Linq;

    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The ford_ fulkerson.
    /// </summary>
    public class Ford_Fulkerson
    {
        /// <summary>
        /// The graph.
        /// </summary>
        private AdjacencyMatrixGraph graph;

        /// <summary>
        /// The nodes in the right part.
        /// </summary>
        private Node[] nodesInTheRightPart;

        /// <summary>
        /// The nodes in the left part.
        /// </summary>
        private Node[] nodesInTheLeftPart;

        /// <summary>
        /// The get maximum matching.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public List<Edge> GetMaximumMatching(AdjacencyMatrixGraph graph)
        {
            // TODO https://www.geeksforgeeks.org/maximum-bipartite-matching/
            this.graph = graph;

            // Split nodes in bipartite graph into two groups basing on their color
            this.nodesInTheRightPart = this.graph.Nodes.Where(n => n.Color == 1).ToArray();
            this.nodesInTheLeftPart = this.graph.Nodes.Where(n => n.Color == 0).ToArray();

            return this.RunEdmondsKarpsAlgorithm();
        }

        /// <summary>
        /// The run edmonds karp algorithm.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        private List<Edge> RunEdmondsKarpsAlgorithm()
        {
            var matching = new List<Edge>();

            // Dictionary with edges that belong in matching.
            // Keys are from right part of graph and values are from left part.
            var rightMatching = new Dictionary<int, int>();

            // Initially the matching is empty
            foreach (var rightNode in this.nodesInTheRightPart)
            {
                rightMatching[rightNode.Id] = -1;
            }

            foreach (var nodeInLeftPart in this.nodesInTheLeftPart)
            {
                foreach (var nodeInRightPart in this.nodesInTheRightPart)
                {
                    nodeInRightPart.Visited = false;
                }

                // Try to find node in right part for node in left part
                this.FindMatchinfForNode(nodeInLeftPart.Id, rightMatching);
            }

            foreach (var m in rightMatching)
            {
                if (m.Value != -1)
                {
                    matching.Add(new Edge(m.Key, m.Value));
                }
            }

            return matching;
        }

        /// <summary>
        /// The find matchinf for node.
        /// </summary>
        /// <param name="nodeId">
        /// The node id.
        /// </param>
        /// <param name="matchR">
        /// The match r.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FindMatchinfForNode(int nodeId, Dictionary<int, int> matchR)
        {
            // Try every job one by one
            foreach (var v in this.nodesInTheRightPart)
            {
                // If applicant u is interested
                // in job v and v is not visited
                if (this.graph.Matrix[nodeId][v.Id] == 1 && !this.graph.Nodes[v.Id].Visited)
                {
                    // Mark v as visited
                    this.graph.Nodes[v.Id].Visited = true;

                    // If job 'v' is not assigned to
                    // an applicant OR previously assigned
                    // applicant for job v (which is matchR[v])
                    // has an alternate job available.
                    // Since v is marked as visited in the above
                    // line, matchR[v] in the following recursive
                    // call will not get job 'v' again
                    if (matchR[v.Id] < 0 || this.FindMatchinfForNode(matchR[v.Id], matchR))
                    {
                        matchR[v.Id] = nodeId;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
