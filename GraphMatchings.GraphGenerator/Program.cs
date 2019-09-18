namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GenerateGraph(11,10);
            //if(n%2 ==1){
            // max edges = n/2 * n+1/2
            // else

        }

        private static void GenerateGraph(int numberOfNodes, int numberOfEdges)
        {
            var random = new Random();
            var rowIndexes = new List<int>();
            var columnIndexes = new List<int>();

            // Split nodes into two partitions
            if (numberOfNodes % 2 == 0)
            {
                for (var node = 0; node < numberOfNodes / 2; node++)
                {
                    rowIndexes.Add(node);
                }

                for (var node = numberOfNodes / 2; node < numberOfNodes; node++)
                {
                    columnIndexes.Add(node);
                }
            }
            else
            {
                for (var node = 0; node < numberOfNodes / 2; node++)
                {
                    rowIndexes.Add(node);
                }

                for (var node = numberOfNodes / 2; node < numberOfNodes; node++)
                {
                    columnIndexes.Add(node);
                }
            }

            var matrix = new int[numberOfNodes, numberOfNodes];

            // Make graph connected
            var notConnectedNodes = rowIndexes.Select(c => c).ToList();

            // TODO najpier połącz
            for (int i = 0; i < columnIndexes.Count; ++i)
            {
                if (notConnectedNodes.Count == 0)
                {
                    notConnectedNodes = rowIndexes.Select(c => c).ToList();
                }

                var e = random.Next(notConnectedNodes.Count-1);
                matrix[notConnectedNodes[e], columnIndexes[i]] = 1;
                matrix[columnIndexes[i], notConnectedNodes[e]] = 1;
                notConnectedNodes.Remove(notConnectedNodes[e]);
            }

            for (var row = 0; row < numberOfNodes; ++row)
            {
                for (var column = 0; column < numberOfNodes; ++column)
                {
                    Console.Write($"{matrix[row, column]} ");
                }

                Console.WriteLine();
            }
            var generatedEdges = 0;
            while (generatedEdges != numberOfEdges)
            {

            }

        }
    }
}
