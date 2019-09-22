namespace GraphMatchings.Console
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandLine;
    using Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(
                    o =>
                        {
                            ParseInputAndRunAlgorithm(o.InputFile);
                        });
        }

        private static void ParseInputAndRunAlgorithm(string pathToFile)
        {
            var isWeighted = false;
            var graph = GraphParser.Parse(pathToFile);
            for (var i = 0; i < graph.GetLength(0); ++i)
            {
                for (var j = 0; j < graph.GetLength(1); ++j)
                {
                    if (graph[i, j] > 1)
                    {
                        isWeighted = true;
                    }
                }
            }

            var matching = new List<Tuple<int, int>>();

            if (isWeighted)
            {
                matching = HungarianMethod.Run(graph);
            }
            else
            {
                matching = FordFulkersonMethod.Run(graph);
            }

            foreach (var edge in matching)
            {
                Console.WriteLine($"{edge.Item1} {edge.Item2}");
            }
        }
    }
}