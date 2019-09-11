namespace GraphMatchings.Core.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class BipartitenessChecker
    {
        public static bool IsGraphBipartite(int[,] graph)
        {
            var colors = new int[graph.GetLength(0)];
            var visited = new bool[graph.GetLength(0)];
            var queue = new Queue<int>();

            visited[0] = true;
            colors[0] = 1;
            queue.Enqueue(0);

            while (queue.Any())
            {
                var u = queue.Dequeue();

                foreach (var w in GetNeighbours(graph, u))
                {
                    if (!visited[w])
                    {
                        visited[w] = true;
                        colors[w] = 3 - colors[u];
                        queue.Enqueue(w);
                    }
                    else if (colors[w] == colors[u])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static IEnumerable<int> GetNeighbours(int[,] graph, int node)
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