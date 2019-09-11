namespace GraphMatchings.Console
{
    using System;

    using CommandLine;

    using GraphMatchings.Core;
    using GraphMatchings.Core.Utils;

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

            if (!GraphChecker.IsGraphBipartite(graph))
            {
                throw new Exception("Given graph is not bipartire");
            }
        }
    }
}