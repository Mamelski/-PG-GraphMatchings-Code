using System;
using System.Collections.Generic;

namespace GraphMatchings.Tester
{
    using System.Diagnostics;
    using System.Linq;

    using GraphMatchings.Core;

    public class SmallGraphsTester
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static readonly Dictionary<string, List<TimeSpan>> BruteForceAlgorithmTimes = new Dictionary<string, List<TimeSpan>>();
        private static readonly Dictionary<string, List<TimeSpan>> FordFulkersonTimes = new Dictionary<string, List<TimeSpan>>();
        private static readonly Dictionary<string, List<TimeSpan>> KuhnMunkresTimes = new Dictionary<string, List<TimeSpan>>();


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
            SortEdgesInFordFulkersonResult(ref fordFulkersonResult);

            var ok = AreResultsTheSame(bruteForceAlgorithmResults, fordFulkersonResult);

            if (!ok)
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
        private static void SortEdgesInFordFulkersonResult(ref List<Tuple<int, int>> fordFulkersonResult)
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