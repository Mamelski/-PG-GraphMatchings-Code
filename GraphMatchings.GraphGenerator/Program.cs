namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static void GenerateGraph(int numberOfNodes, int numberOfEdges)
        {
            var random = new Random();
            var colors = new int[numberOfNodes];

            // Give random color
            for (var node = 0; node < numberOfNodes; ++numberOfNodes)
            {
                colors[node] = random.Next(2) + 1;

                // TODO dodaj
            }

            var rowIndexes = new List<int>();
            var columnIndexes = new List<int>();


        var matrix = new int[numberOfNodes, numberOfNodes];

            var generatedEdges = 0;
            while (generatedEdges != numberOfEdges)
            {

            }

        }
    }
}
