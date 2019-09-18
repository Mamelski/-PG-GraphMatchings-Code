namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;

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

            // TODO najpier połącz


            var matrix = new int[numberOfNodes, numberOfNodes];

            var generatedEdges = 0;
            while (generatedEdges != numberOfEdges)
            {

            }

        }
    }
}
