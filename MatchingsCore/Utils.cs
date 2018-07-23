namespace MatchingsCore
{
    using MatchingsCore.GraphRepresentation;

    /// <summary>
    /// The utils.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// The is bipartite.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ColortGraphAndCheckIfItIsBipartite(ref Graph graph)
        {
            var firstNode = graph[0];

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

            for (var i = 0; i < node.Edges.Count; ++i)
            {
                var neighour = node.Edges[i].Destination;

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
