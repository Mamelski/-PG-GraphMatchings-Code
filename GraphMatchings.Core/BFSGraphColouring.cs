namespace GraphMatchings.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using GraphMatchings.Core.Utils;

    public class BFSGraphColouring
    {
        public static int[] Run(int[,] graph)
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

                foreach (var w in GraphHelper.GetNeighbours(graph, u))
                {
                    if (!visited[w])
                    {
                        visited[w] = true;
                        colors[w] = 3 - colors[u];
                        queue.Enqueue(w);
                    }
                }
            }

            return colors;
        }
    }
}
