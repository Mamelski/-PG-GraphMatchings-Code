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

            return isGraphBipartite;
        }

        /// <summary>
        /// The dfs with two colors.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool DFSWithTwoColors(ref Node node, int color)
        {
            for (int i = 0; i < node.Neighbours.Count; ++i)
            {
                var neighour = node.Neighbours[i];

                if (!neighour.Visited)
                {
                    neighour.Visited = true;
                

                    // We have two colors
                    color = color == 0 ? 1 : 0;
                    neighour.Color = color;

                    return this.DFSWithTwoColors(ref neighour, color);
                }
                else
                {
                    if (node.Color == neighour.Color)
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
