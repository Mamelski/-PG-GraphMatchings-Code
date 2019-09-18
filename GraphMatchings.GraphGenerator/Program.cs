namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {

        private static bool isWeighted;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var x =GenerateGraph(10, 10);
            int a = 0;
        }

        private static int[,] GenerateGraph(int numberOfNodes, int numberOfEdges)
        {
            var random = new Random();
            var rowIndexes = new List<int>();
            var columnIndexes = new List<int>();

            // Split nodes into two partitions
            for (var node = 0; node < numberOfNodes / 2; node++)
            {
                rowIndexes.Add(node);
            }

            for (var node = numberOfNodes / 2; node < numberOfNodes; node++)
            {
                columnIndexes.Add(node);
            }

            var matrix = new int[numberOfNodes, numberOfNodes];

            // Make graph connected
            var notConnectedNodes = rowIndexes.Select(c => c).ToList();

            foreach (var node in columnIndexes)
            {
                if (notConnectedNodes.Count == 0)
                {
                    notConnectedNodes = rowIndexes.Select(c => c).ToList();
                }

                var neighbour = random.Next(notConnectedNodes.Count - 1);

                var weight = isWeighted ? random.Next(10000) : 1;


                matrix[notConnectedNodes[neighbour], node] = weight;
                matrix[node, notConnectedNodes[neighbour]] = weight;

                notConnectedNodes.Remove(notConnectedNodes[neighbour]);
            }

            var numberOfEdgesToAdd = numberOfEdges - columnIndexes.Count;
            for (var i = 0; i < numberOfEdgesToAdd; ++i)
            {
                var isHit = false;

                while (!isHit)
                {
                    var node1 = random.Next(columnIndexes.Count);
                    var node2 = random.Next(rowIndexes.Count);

                    if (matrix[columnIndexes[node1], rowIndexes[node2]] != 0)
                    {
                        continue;
                    }

                    var weight = isWeighted ? random.Next(10000) : 1;

                    matrix[columnIndexes[node1], rowIndexes[node2]] = weight;
                    matrix[rowIndexes[node2], columnIndexes[node1]] = weight;
                    isHit = true;
                }
            }

            return matrix;
        }
    }
}
