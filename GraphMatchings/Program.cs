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

            if (!BipartitenessChecker.IsGraphBipartite(graph))
            {
                throw new Exception("Given graph is not bipartire");
            }

            if (WeightsChecker.IsGraphWeighted(graph))
            {
                HungarianMethod.Run(graph);
            }
            else
            {
                Ford_FulkersonMethod.Run(graph);
            }
        }
    }
}