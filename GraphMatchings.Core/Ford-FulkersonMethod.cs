namespace GraphMatchings.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using GraphMatchings.Core.Utils;

    public class Ford_FulkersonMethod
    {
        public static void Run(int[,] graph)
        {
            var flowNetwork = TranformGraphToFlowNetwork(graph);
            var path = FindAugmentingPath(flowNetwork);
        }

        private static int[,] TranformGraphToFlowNetwork(int[,] graph)
        {
            var source = graph.GetLength(0);
            var sink = graph.GetLength(0) + 1;
            var flowNetworkNodesNumber = graph.GetLength(0) + 2;

            var flowNetwork = new int[flowNetworkNodesNumber, flowNetworkNodesNumber];

            var colors = BFSGraphColouring.Run(graph);

            for (var v = 0; v < flowNetworkNodesNumber - 2; ++v)
            {
                if (colors[v] == v)
                {
                    flowNetwork[source, v] = 1;
                    foreach (var u in GraphHelper.GetNeighbours(graph, v))
                    {
                        flowNetwork[v, u] = 1;
                    }
                }
                else
                {
                    flowNetwork[v, sink] = 1;
                }
            }

            return flowNetwork;
        }

        private static List<int> FindAugmentingPath(int[,] flowNetwork)
        {
            var source = flowNetwork.GetLength(0) - 2;
            var sink = flowNetwork.GetLength(0) - 1;
            var visited = new bool[flowNetwork.GetLength(0)];
            var parents = new int[flowNetwork.GetLength(0)];
            var queue = new Queue<int>();

            visited[source] = true;
            queue.Enqueue(source);

            while (queue.Any())
            {
                var v = queue.Dequeue();

                foreach (var w in GraphHelper.GetNeighbours(flowNetwork,v))
                {
                    if (w == sink)
                    {
                        visited[sink] = true;
                        parents[sink] = v;
                        queue.Clear();
                        break;
                    }

                    // TODO
                }
            }

            return BuildPath(parents, sink);
        }

        private static List<int> BuildPath(int[] parents, int sink)
        {
            return null;
        }
    }
}
