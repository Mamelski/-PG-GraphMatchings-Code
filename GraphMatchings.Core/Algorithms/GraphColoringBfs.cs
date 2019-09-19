namespace GraphMatchings.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using Utils;

    public static class GraphColoringBfs
    {
        public static int[] Run(int[,] graph)
        {
            var colors = new int[GraphHelper.NumberOfNodes(graph)];
            var visited = new bool[GraphHelper.NumberOfNodes(graph)];
            var queue = new Queue<int>();
            var startingNode = 0;

            visited[startingNode] = true;
            colors[startingNode] = 1;
            queue.Enqueue(startingNode);

            while (queue.Any())
            {
                var node = queue.Dequeue();

                foreach (var neighbour in GraphHelper.GetNeighbours(graph, node))
                {
                    if (!visited[neighbour])
                    {
                        visited[neighbour] = true;

                        // Changes 1 to 2 and 2 to 1
                        colors[neighbour] = 3 - colors[node];
                        queue.Enqueue(neighbour);
                    }
                }
            }

            return colors;
        }
    }
}
