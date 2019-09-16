namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Utils;

    public static class FordFulkersonMethod
    {
        // Entry point, runs Edmonds-Karp algorithm for given graph
        // Returns collection of edges that are maximum weighted matching
        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            var flowNetwork = TranformGraphToFlowNetwork(graph, out var colors);

            var path = FindAugmentingPath(flowNetwork);
            while (path.Any())
            {
                SendFlow(flowNetwork, path);
                path = FindAugmentingPath(flowNetwork);
            }

            var matching = ReadMatching(flowNetwork, colors);
            return matching;
        }

        private static int[,] TranformGraphToFlowNetwork(int[,] graph, out int[] colors)
        {
            var source = GraphHelper.NumberOfNodes(graph);
            var sink = GraphHelper.NumberOfNodes(graph) + 1;
            var flowNetworkNodesNumber = GraphHelper.NumberOfNodes(graph) + 2;

            // New modified graph
            var flowNetwork = new int[flowNetworkNodesNumber, flowNetworkNodesNumber];

            colors = GraphColoringBfs.Run(graph);

            for (var node = 0; node < colors.Length; ++node)
            {
                // Node is in first partition
                if (colors[node] == 1)
                {
                    // Add edge from source
                    flowNetwork[source, node] = 1;

                    // Add directed edge to all neighbours (they are in partition 2)
                    foreach (var neighbour in GraphHelper.GetNeighbours(graph, node))
                    {
                        flowNetwork[node, neighbour] = 1;
                    }
                }
                else
                {
                    // Node is in second partition, add edge to sink
                    flowNetwork[node, sink] = 1;
                }
            }

            return flowNetwork;
        }

        private static List<Tuple<int, int>> FindAugmentingPath(int[,] flowNetwork)
        {
            var source = GraphHelper.NumberOfNodes(flowNetwork) - 2;
            var sink = GraphHelper.NumberOfNodes(flowNetwork) - 1;

            var visited = new bool[GraphHelper.NumberOfNodes(flowNetwork)];
            var parents = new int[GraphHelper.NumberOfNodes(flowNetwork)];
            var queue = new Queue<int>();

            // Starting from source
            visited[source] = true;
            queue.Enqueue(source);

            // While any node to visit
            while (queue.Any())
            {
                var node = queue.Dequeue();

                foreach (var neighbour in GraphHelper.GetNeighbours(flowNetwork, node))
                {
                    // We found sink
                    if (neighbour == sink)
                    {
                        visited[sink] = true;
                        parents[sink] = node;
                        queue.Clear();
                        break;
                    }

                    if (!visited[neighbour])
                    {
                        visited[neighbour] = true;
                        parents[neighbour] = node;
                        queue.Enqueue(neighbour);
                    }
                }
            }

            // If we did not reach sink return empty path, otherwise return path
            return !visited[sink] ? new List<Tuple<int, int>>()  : BuildPath(parents, source, sink);
        }

        private static List<Tuple<int, int>> BuildPath(int[] parents, int source, int sink)
        {
            var path = new List<Tuple<int, int>>();
            var current = sink;

            // Going back from sink to source adding edges to path, based on parents
            while (current != source)
            {
                path.Add(new Tuple<int, int>(parents[current], current));
                current = parents[current];
            }

            path.Reverse();

            return path;
        }

        private static void SendFlow(int[,] graph, List<Tuple<int, int>> path)
        {
            // Reverse edge for each edge in a path
            foreach (var edge in path)
            {
                graph[edge.Item1, edge.Item2] = 0;
                graph[edge.Item2, edge.Item1] = 1;
            }
        }

        private static List<Tuple<int, int>> ReadMatching(int[,] flowNetwork, int[] colors)
        {
            var matching = new List<Tuple<int, int>>();
            var sink = GraphHelper.NumberOfNodes(flowNetwork) - 1;

            for (var node = 0; node < GraphHelper.NumberOfNodes(flowNetwork) - 2; ++node)
            {
                // Node from second partition
                if (colors[node] == 2)
                {
                    // Find reversed edges and add to matching
                    foreach (var u in GraphHelper.GetNeighbours(flowNetwork, node))
                    {
                        if (u != sink)
                        {
                            matching.Add(new Tuple<int, int>(u, node));
                        }
                    }
                }
            }

            return matching;
        }
    }
}
