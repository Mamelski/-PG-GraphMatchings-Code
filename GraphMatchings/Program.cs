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
            var isWeighted = false;
            var graph = GraphParser.Parse(pathToFile, ref isWeighted);

            if (!BipartitenessChecker.IsGraphBipartite(graph))
            {
                throw new Exception("Given graph is not bipartire");
            }

            if (isWeighted)
            {
                //var matching = HungarianMethod.Run(graph);
                BruteForceMatchingAlgorithm.Run(graph);
            }
            else
            {
                var matching = FordFulkersonMethod.Run(graph);
            }

            // TODO print
        }
    }
}