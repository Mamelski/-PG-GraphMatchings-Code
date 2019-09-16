namespace GraphMatchings.Core.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class BipartitenessChecker
    {
        public static bool IsGraphBipartite(int[,] graph)
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
                        colors[neighbour] = 3 - colors[node];
                        queue.Enqueue(neighbour);
                    }
                    else if (colors[neighbour] == colors[node])
                    {
                        // Neighbours cant have the same color in bipartite graph
                        return false;
                    }
                }
            }

            return true;
        }
    }
}