using System;
using System.Collections.Generic;

namespace GraphMatchings.Tester
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GraphMatchings.Core;
    using GraphMatchings.Core.Utils;

    public class SmallGraphsTester
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static readonly Dictionary<string, List<TimeSpan>> BruteForceAlgorithmTimes = new Dictionary<string, List<TimeSpan>>();
        private static readonly Dictionary<string, List<TimeSpan>> FordFulkersonTimes = new Dictionary<string, List<TimeSpan>>();
        private static readonly Dictionary<string, List<TimeSpan>> KuhnMunkresTimes = new Dictionary<string, List<TimeSpan>>();

        public static void TestSmallGraphs(string myFormatDirectory)
        {
            foreach (var filePath in Directory.GetFiles(myFormatDirectory))
            {
                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";

                var graph = GraphParser.Parse(filePath);

                RunAndCheckResults(fileType, graph);
            }

            PrintResult();
        }

        private static void PrintResult()
        {
            throw new NotImplementedException();
        }

        public static void RunAndCheckResults(string fileType, int[,] graph)
        {
            AddFileTypeToDictionaryIfNeeded(fileType);

            Stopwatch.Reset();
            Stopwatch.Start();
            var bruteForceAlgorithmResults = BruteForceMatchingAlgorithm.Run(graph);
            Stopwatch.Stop();
            BruteForceAlgorithmTimes[fileType].Add(Stopwatch.Elapsed);

            Stopwatch.Reset();
            Stopwatch.Start();
            var fordFulkersonResult = FordFulkersonMethod.Run(graph);
            Stopwatch.Stop();
            FordFulkersonTimes[fileType].Add(Stopwatch.Elapsed);

            Stopwatch.Reset();
            Stopwatch.Start();
            var kuhnMunkersResult = HungarianMethod.Run(graph);
            Stopwatch.Stop();
            FordFulkersonTimes[fileType].Add(Stopwatch.Elapsed);

            SortEdgesInBruteForceResults(ref bruteForceAlgorithmResults);
            SortEdgesResult(ref fordFulkersonResult);
            SortEdgesResult(ref kuhnMunkersResult);

            var fordFulkersonOk = AreResultsTheSame(bruteForceAlgorithmResults, fordFulkersonResult);
            var kuhnMunkersOk = AreResultsTheSame(bruteForceAlgorithmResults, kuhnMunkersResult);

            if (!fordFulkersonOk || !kuhnMunkersOk)
            {
                Console.WriteLine("Not working");
            }
        }

        private static void AddFileTypeToDictionaryIfNeeded(string fileType)
        {
            if (!BruteForceAlgorithmTimes.ContainsKey(fileType))
            {
                BruteForceAlgorithmTimes.Add(fileType, new List<TimeSpan>());
            }

            if (!FordFulkersonTimes.ContainsKey(fileType))
            {
                FordFulkersonTimes.Add(fileType, new List<TimeSpan>());
            }

            if (!KuhnMunkresTimes.ContainsKey(fileType))
            {
                KuhnMunkresTimes.Add(fileType, new List<TimeSpan>());
            }
        }

        // Check if result from normal algorithm is present in results from bruteforce algorithm
        private static bool AreResultsTheSame(List<List<Tuple<int, int>>> bruteForceAlgorithmResults, List<Tuple<int, int>> normalAlgorithmResults)
        {
            foreach (var result in bruteForceAlgorithmResults)
            {
                if (result.Count == normalAlgorithmResults.Count)
                {
                    if (result.All(normalAlgorithmResults.Contains))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Sort edges
        private static void SortEdgesInBruteForceResults(ref List<List<Tuple<int, int>>> bruteForceAlgorithmResults)
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

        // Sort edges
        private static void SortEdgesResult(ref List<Tuple<int, int>> result)
        {
            for (var i = 0; i < result.Count; ++i)
            {
                if (result[i].Item1 > result[i].Item2)
                {
                    result[i] = new Tuple<int, int>(
                        result[i].Item2,
                        result[i].Item1);
                }
            }
        }
    }
}