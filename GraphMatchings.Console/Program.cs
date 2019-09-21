namespace GraphMatchings.Console
{
    using System;
    using System.Linq;
    using System.Security.Principal;

    using CommandLine;

    using Core;
    using Core.Utils;

    using Microsoft.VisualBasic.CompilerServices;

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

            var bruteForceOutput = BruteForceMatchingAlgorithm.Run(graph);
            var fordFulkersonOutput = FordFulkersonMethod.Run(graph);


            // Sort bruteforce
            foreach (var result in bruteForceOutput)
            {
                for(var i =0; i < result.Count;++i)
                {
                    if (result[i].Item1 > result[i].Item2)
                    {
                        result[i] = new Tuple<int, int>(result[i].Item2, result[i].Item1);
                    }
                }
            }

            // Sort Ford output
            for (var i = 0; i < fordFulkersonOutput.Count; ++i)
            {
                if (fordFulkersonOutput[i].Item1 > fordFulkersonOutput[i].Item2)
                {
                    fordFulkersonOutput[i] = new Tuple<int, int>(fordFulkersonOutput[i].Item2, fordFulkersonOutput[i].Item1);
                }
            }

            var isOK = false;
            // Check
            foreach (var result in bruteForceOutput)
            {
                if (result.Count == fordFulkersonOutput.Count)
                {
                    if (result.All(r => fordFulkersonOutput.Contains(r)))
                    {
                        isOK = true;
                    }
                }
            }


            if (isWeighted)
            {
                //var matching = HungarianMethod.Run(graph);
                BruteForceMatchingAlgorithm.Run(graph);
            }
            else
            {
                //var matching = FordFulkersonMethod.Run(graph);
                BruteForceMatchingAlgorithm.Run(graph);
            }

            // TODO print
        }
    }
}