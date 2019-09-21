using System;
using System.Collections.Generic;

namespace GraphMatchings.Tester
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using GraphMatchings.Core;

    public class SmallGraphsTester
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static readonly Dictionary<string, List<long>> BruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
        private static readonly Dictionary<string, List<long>> FordFulkersonTimes = new Dictionary<string, List<long>>();
        private static readonly Dictionary<string, List<long>> KuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void TestSmallGraphs(string myFormatDirectory)
        {
            foreach (var filePath in Directory.GetFiles(myFormatDirectory))
            {
              //  Console.WriteLine(Path.GetFileNameWithoutExtension(filePath));

                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";

                var graph = GraphParser.Parse(filePath);

                RunAndCheckResults(fileType, graph);
            }

            PrintResult();
        }

        private static void PrintResult()
        {
            var outputPath = @"SmallTestsResults\results.txt";
            using (var sw = File.CreateText(outputPath))
            {
                sw.WriteLine("file\tsumOfNodes\tnumberOfTests\tBruteForce\tFordFulkerson\tKuhnMunkers\tMicroseconds");
                foreach (var key in BruteForceAlgorithmTimes.Keys)
                {
                    var avgBruteForce = BruteForceAlgorithmTimes[key].Average(ticks => (ticks * 1000000) / Stopwatch.Frequency);
                    var avgFordFulkerson = FordFulkersonTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                    var avgKuhnMunkres = KuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));

                    var split = key.Split('-');

                    var sumOfNodes = int.Parse(split[0]) + int.Parse(split[1]);
                    sw.WriteLine($"{key}\t{sumOfNodes}\t{BruteForceAlgorithmTimes[key].Count}\t{Math.Round(avgBruteForce,4)}\t{Math.Round(avgFordFulkerson,4)}\t{Math.Round(avgKuhnMunkres,4)}");
                }
            }
        }

        public static void RunAndCheckResults(string fileType, int[,] graph)
        {
            //if (fileType.Equals("2-2"))
            //{
            //    int a = 0;
            //}

            AddFileTypeToDictionaryIfNeeded(fileType);

            
            Stopwatch.Start();
            var bruteForceAlgorithmResults = BruteForceMatchingAlgorithm.Run(graph);
            Stopwatch.Stop();
            BruteForceAlgorithmTimes[fileType].Add(Stopwatch.ElapsedTicks);

            Stopwatch.Reset();
            Stopwatch.Start();
            var fordFulkersonResult = FordFulkersonMethod.Run(graph);
            Stopwatch.Stop();
            FordFulkersonTimes[fileType].Add(Stopwatch.ElapsedTicks);

            Stopwatch.Reset();
            Stopwatch.Start();
            var kuhnMunkersResult = HungarianMethod.Run(graph);
            Stopwatch.Stop();
            KuhnMunkresTimes[fileType].Add(Stopwatch.ElapsedTicks);

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
                BruteForceAlgorithmTimes.Add(fileType, new List<long>());
            }

            if (!FordFulkersonTimes.ContainsKey(fileType))
            {
                FordFulkersonTimes.Add(fileType, new List<long>());
            }

            if (!KuhnMunkresTimes.ContainsKey(fileType))
            {
                KuhnMunkresTimes.Add(fileType, new List<long>());
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