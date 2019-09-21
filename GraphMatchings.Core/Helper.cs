namespace GraphMatchings.Core
{
    using System.Collections.Generic;

    public static class Helper
    {
        public static IEnumerable<int> GetNodeNeighbours(int[,] graph, int node)
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

        public static int GetNumberOfNodes(int[,] graph)
        {
            return graph.GetLength(0);
        }

        public static int GetRowsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(0);
        }

        public static int GetColumnsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(1);
        }
    }
}
