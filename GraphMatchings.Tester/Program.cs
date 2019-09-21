namespace GraphMatchings.Tester
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;

    using GraphMatchings.Core;
    using GraphMatchings.Core.Utils;

    public class Program
    {
        private const string MyFormatDirectory = @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphGenerator\bin\Debug\netcoreapp2.1\SmallGraphs\MyFormat\";
        private static Stopwatch sw1 = new Stopwatch();
       // private static Stopwatch sw2 = new Stopwatch();
        private static TimeSpan BTime = TimeSpan.Zero;

        private static Dictionary<string, TimeSpan> bruteForceAlgorithmTimes = new Dictionary<string, TimeSpan>();
        private static Dictionary<string, TimeSpan> fordFulkersonTimes = new Dictionary<string, TimeSpan>();

        private static TimeSpan FTime = TimeSpan.Zero;

        public static void Main(string[] args)
        {
            TestSmallGraph();
            Console.WriteLine("Hello World!");
        }

        private static void TestSmallGraph()
        {
            var c = 0;
            foreach (var filePath in Directory.GetFiles(MyFormatDirectory))
            {
                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";
               // Console.WriteLine(c);
               // Console.WriteLine(Path.GetFileName(filePath));

                var graph = GraphParser.Parse(filePath);

               // MatrixHelper.PrintMatrix(graph);
                 RunAndCheckResults(fileType, graph);
                ++c;
            }
        }

        private static void RunAndCheckResults(string fileType, int[,] graph)
        {
            if (!bruteForceAlgorithmTimes.ContainsKey(fileType))
            {
                bruteForceAlgorithmTimes.Add(fileType, TimeSpan.Zero);
            }

            if (!fordFulkersonTimes.ContainsKey(fileType))
            {
                fordFulkersonTimes.Add(fileType, TimeSpan.Zero);
            }

            sw1.Reset();
            sw1.Start();
            var bruteForceAlgorithmResults = BruteForceMatchingAlgorithm.Run(graph);
            sw1.Stop();

            bruteForceAlgorithmTimes[fileType] += sw1.Elapsed;
            BTime += sw1.Elapsed;

            sw1.Reset();
            sw1.Start();
            var fordFulkersonResult = FordFulkersonMethod.Run(graph);
            sw1.Stop();

            fordFulkersonTimes[fileType] += sw1.Elapsed;
            FTime += sw1.Elapsed;

            NormalizeBruteforceResults(ref bruteForceAlgorithmResults);
            NormalizeFordFulkersonResult(ref fordFulkersonResult);

            var ok = IsResultTheSame(bruteForceAlgorithmResults, fordFulkersonResult);

            if (!ok)
            {
                Console.WriteLine("Not working");
            }
        }

        private static bool IsResultTheSame(List<List<Tuple<int, int>>> bruteForceAlgorithmResults, List<Tuple<int, int>> fordFulkersonResult)
        {
            foreach (var result in bruteForceAlgorithmResults)
            {
                if (result.Count == fordFulkersonResult.Count)
                {
                    if (result.All(fordFulkersonResult.Contains))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void NormalizeBruteforceResults(ref List<List<Tuple<int, int>>> bruteForceAlgorithmResults)
        {
            foreach (var result in bruteForceAlgorithmResults)
            {
                for (var i = 0; i < result.Count; ++i)
                {
                    if (result[i].Item1 > result[i].Item2)
                    {
                        result[i] = new Tuple<int, int>(result[i].Item2, result[i].Item1);
                    }
                }
            }
        }

        private static void NormalizeFordFulkersonResult(ref List<Tuple<int, int>> fordFulkersonResult)
        {
            for (var i = 0; i < fordFulkersonResult.Count; ++i)
            {
                if (fordFulkersonResult[i].Item1 > fordFulkersonResult[i].Item2)
                {
                    fordFulkersonResult[i] = new Tuple<int, int>(
                        fordFulkersonResult[i].Item2,
                        fordFulkersonResult[i].Item1);
                }
            }
        }

    }
}

