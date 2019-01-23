using GraphMatchings.Core.GraphRepresentation;

namespace MatchingsCore.Miscellaneous
{
    using System.Collections.Generic;

    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The utils.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// The number of nodes.
        /// </summary>
        private int numberOfNodes;

        /// <summary>
        /// The graph.
        /// </summary>
        private AdjacencyMatrixGraph graph;

        /// <summary>
        /// The is bipartite.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ColortGraphAndCheckIfItIsBipartite(ref AdjacencyMatrixGraph graph)
        {
            this.numberOfNodes = graph.Nodes.Count;
            this.graph = graph;
            var firstNode = graph.Nodes[0];

            var isGraphBipartite = this.DFSWithTwoColors(ref firstNode, 0);

            // TODO sprawdz czy cykl
            // TODO co jeśli nie graf
            return isGraphBipartite;
        }

        /// <summary>
        /// The dfs with two colors.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="currentColor">
        /// The currentColor.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DFSWithTwoColors(ref Node node, int currentColor)
        {
            node.Visited = true;

            var neighboursColor = currentColor == 0 ? 1 : 0;

            for (var i = 0; i < this.numberOfNodes; ++i)
            {
                if (this.graph.Matrix[node.Id][i] == 0)
                {
                    continue;
                }

                var neighour = this.graph.Nodes[i];

                if (!neighour.Visited)
                {
                    neighour.Color = neighboursColor;

                    if (!this.DFSWithTwoColors(ref neighour, neighboursColor))
                    {
                        return false;
                    }
                }
                else
                {
                    if (node.Color == neighour.Color)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
