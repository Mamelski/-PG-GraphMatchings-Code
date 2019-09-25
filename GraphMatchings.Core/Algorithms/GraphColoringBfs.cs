namespace GraphMatchings.Core.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GraphColoringBfs
    {
        public static int[] Run(int[,] graph)
        {
            var colors = new int[Helper.GetNumberOfNodes(graph)];
            var visited = new bool[Helper.GetNumberOfNodes(graph)];
            var queue = new Queue<int>();
            var startingNode = 0;

            visited[startingNode] = true;
            colors[startingNode] = 1;
            queue.Enqueue(startingNode);

            while (queue.Any())
            {
                var node = queue.Dequeue();

                foreach (var neighbour in Helper.GetNodeNeighbours(graph, node))
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
