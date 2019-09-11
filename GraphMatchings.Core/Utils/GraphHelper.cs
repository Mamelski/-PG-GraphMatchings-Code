namespace GraphMatchings.Core.Utils
{
    using System.Collections.Generic;

    public class GraphHelper
    {
        public static IEnumerable<int> GetNeighbours(int[,] graph, int node)
        {
            var neighbours = new List<int>();

            for (var i = 0; i < graph.GetLength(1); ++i)
            {
                if (graph[node, i] != 0)
                {
                    neighbours.Add(i);
                }
            }

            return neighbours;
        }
    }
}
