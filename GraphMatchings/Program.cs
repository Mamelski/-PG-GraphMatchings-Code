namespace GraphMatchings.Console
{
    using System;

    using CommandLine;

    using Core;
    using Core.Utils;

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
            var graph = GraphParser.Parse(pathToFile);
            MatrixHelper.PrintMatrix(graph);

            if (!BipartitenessChecker.IsGraphBipartite(graph))
            {
                throw new Exception("Given graph is not bipartire");
            }

            if (WeightsChecker.IsGraphWeighted(graph))
            {
                var matching = HungarianMethod.Run(graph);
            }
            else
            {
               var matching = FordFulkersonMethod.Run(graph);
            }
        }
    }
}