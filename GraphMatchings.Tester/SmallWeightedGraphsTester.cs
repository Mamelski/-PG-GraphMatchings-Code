using System;
using System.Collections.Generic;

namespace GraphMatchings.Tester
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using GraphMatchings.Core;

    public class SmallWeightedGraphsTester
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();
        private static readonly Random random = new Random();

        private static Dictionary<string, List<long>> BruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> KuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void Test(string myFormatDirectory)
        {
            var possibleWeights = new List<Tuple<int, int>>
                                      {
                                          new Tuple<int, int>(1, 100),
                                          new Tuple<int, int>(1, 1000),
                                          new Tuple<int, int>(1, 10000),
                                          new Tuple<int, int>(1, 100000),
                                          new Tuple<int, int>(1000, 10000),
                                          new Tuple<int, int>(10000, 100000)
                                      };

            foreach (var pw in possibleWeights)
            {
                CleanupResults();
                foreach (var filePath in Directory.GetFiles(myFormatDirectory))
                {

                    var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                    var fileType = $"{split[0]}-{split[1]}";

                    var graph = GraphParser.Parse(filePath);
                    AddWeights(ref graph, 1, 1000);

                    RunAndCheckResults(fileType, graph);
                }

                PrintResult($"weighted_With3dummy-{pw.Item1}-{pw.Item2}.txt");
            }
        }

        private static void PrintResult(string fileName)
        {
            var outputPath = $"SmallWeightedTestsResults\\{fileName}";

            var results = new List<TestResults>();
            foreach (var key in BruteForceAlgorithmTimes.Keys)
            {
                var avgBruteForce = BruteForceAlgorithmTimes[key].Average(ticks => (ticks * 1000000) / Stopwatch.Frequency);

                var avgKuhnMunkres = KuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]) + int.Parse(split[1]),
                                    NumberOfTests = BruteForceAlgorithmTimes[key].Count,
                                    AvgBruteForce = Math.Round(avgBruteForce, 0),
                                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 0)
                                });
            }

            var sortedResults = results.OrderBy(r => r.NumberOfNodes);

            using (var sw = File.CreateText(outputPath))
            {
                sw.WriteLine("file\tsumOfNodes\tnumberOfTests\tBruteForce\tKuhnMunkers\tMicroseconds");
                foreach (var result in sortedResults)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgBruteForce}\t{result.AvgKuhnMunkres}");
                }

            }
        }

        public static void RunAndCheckResults(string fileType, int[,] graph)
        {
            AddFileTypeToDictionaryIfNeeded(fileType);

            Stopwatch.Start();
            var bruteForceAlgorithmResults = BruteForceMatchingAlgorithm.Run(graph);
            Stopwatch.Stop();
            BruteForceAlgorithmTimes[fileType].Add(Stopwatch.ElapsedTicks);

            Stopwatch.Reset();
            Stopwatch.Start();
            var kuhnMunkersResult = HungarianMethod.Run(graph);
            Stopwatch.Stop();
            KuhnMunkresTimes[fileType].Add(Stopwatch.ElapsedTicks);

            SortEdgesInBruteForceResults(ref bruteForceAlgorithmResults);
            SortEdgesResult(ref kuhnMunkersResult);

            var kuhnMunkersOk = AreResultsTheSame(bruteForceAlgorithmResults, kuhnMunkersResult);

            if (!kuhnMunkersOk)
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

        private static void CleanupResults()
        {
            BruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
            KuhnMunkresTimes = new Dictionary<string, List<long>>();
        }

        private static void AddWeights(ref int[,] graph, int min, int max)
        {
            for (int i = 0; i < graph.GetLength(0); ++i)
            {
                for(var j = i; j < graph.GetLength(1); ++j)
                {
                    if (graph[i, j] > 0)
                    {
                        var value = random.Next(min, max);
                        graph[i, j] = value;
                        graph[j, i] = value;
                    }
                }
            }
        }
    }
}