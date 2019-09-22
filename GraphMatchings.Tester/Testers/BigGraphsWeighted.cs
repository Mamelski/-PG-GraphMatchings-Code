namespace GraphMatchings.Tester.Testers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GraphMatchings.Core;

    public static class BigGraphsWeighted
    {
        private static readonly Stopwatch Sw = new Stopwatch();
        private static Dictionary<string, List<long>> bruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> fordFulkersonTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> kuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void Test(string inputDirectory)
        {

            CleanUpTimes();
            TestKuhnMunkres(inputDirectory);
        }

        private static void TestBruteForce(string myFormatDirectory)
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

            var filesToTest = Path.Combine(myFormatDirectory, "20-0.txt");

            foreach (var pw in possibleWeights)
            {
                var fileType = TestHelper.GetFileType(filesToTest);
                Console.WriteLine(fileType);

                var graph = GraphParser.Parse(filesToTest);
                TestHelper.AddWeights(ref graph, pw.Item1, pw.Item2);

                RunBruteForce(fileType, graph);

                var sortedResults = PrepareWeightedBruteForceSortedResults();
                PrintWeightedBruteForces(sortedResults, $"BigGraphs_Weighted_BruteForce_20_0_weights_{pw.Item1}_{pw.Item2}.txt");
            }
        }

        private static void RunBruteForce(string fileType, int[,] graph)
        {
            AddFileTypeToDictionaryIfNeeded(fileType);

            Sw.Reset();
            Sw.Start();
            BruteForceMatchingAlgorithm.Run(graph);
            Sw.Stop();

            bruteForceAlgorithmTimes[fileType].Add(Sw.ElapsedTicks);
        }

        private static List<TestResults> PrepareWeightedBruteForceSortedResults()
        {
            var results = new List<TestResults>();
            foreach (var key in bruteForceAlgorithmTimes.Keys)
            {
                var avgBruteForce = bruteForceAlgorithmTimes[key].Average(ticks => ((ticks * 1) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]) + int.Parse(split[1]),
                                    NumberOfTests = bruteForceAlgorithmTimes[key].Count,
                                    AvgBruteForce = Math.Round(avgBruteForce, 0)
                                });
            }

            return results.OrderBy(r => r.NumberOfNodes).ToList();
        }

        private static void PrintWeightedBruteForces(IEnumerable<TestResults> results, string outputFile)
        {
            using (var sw = File.CreateText($"Results\\{outputFile}"))
            {
                sw.WriteLine("NumberOfNodes\tFileType\tNumberOfTests\tAvgBruteForce");
                sw.WriteLine("Seconds;");
                foreach (var result in results)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgBruteForce}");
                }
            }
        }

        private static void TestKuhnMunkres(string myFormatDirectory)
        {
            var possibleWeights = new List<Tuple<int, int>>
                                      {
                                          new Tuple<int, int>(1, 100),
                                          new Tuple<int, int>(1, 1000),
                                          new Tuple<int, int>(1, 10000),
                                          new Tuple<int, int>(1000, 10000),
                                      };
            foreach (var pw in possibleWeights)
            {
                CleanUpTimes();
                foreach (var filePath in Directory.GetFiles(myFormatDirectory))
                {
                    if (!Path.GetFileNameWithoutExtension(filePath).EndsWith("0"))
                    {
                        continue;
                    }

                    var fileType = Path.GetFileNameWithoutExtension(filePath).Split("-")[0];
                    Console.WriteLine(fileType);

                    var graph = GraphParser.Parse(filePath);
                    TestHelper.AddWeights(ref graph, pw.Item1, pw.Item2);

                    RunKuhnMunkres(fileType, graph);
                }

                var sortedResults = PrepareWeightedKuhnMunkresSortedResults();
                PrintWeightedKuhnMunkres(
                    sortedResults,
                    $"BigGraphs_Weighted_KuhnMunkres_weights_{pw.Item1}_{pw.Item2}.txt");
            }
        }

        private static void RunKuhnMunkres(string fileType, int[,] graph)
        {
            AddFileTypeToDictionaryIfNeeded(fileType);

            Sw.Reset();
            Sw.Start();
            HungarianMethod.Run(graph);
            Sw.Stop();

            kuhnMunkresTimes[fileType].Add(Sw.ElapsedTicks);
        }

        private static List<TestResults> PrepareWeightedKuhnMunkresSortedResults()
        {
            var results = new List<TestResults>();
            foreach (var key in kuhnMunkresTimes.Keys)
            {
                var avgKuhnMunkres = kuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]),
                                    NumberOfTests = kuhnMunkresTimes[key].Count,
                                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 2)
                                });
            }

            return results.OrderBy(r => r.NumberOfNodes).ToList();
        }

        private static void PrintWeightedKuhnMunkres(IEnumerable<TestResults> results, string outputFile)
        {
            using (var sw = File.CreateText($"Results\\{outputFile}"))
            {
                sw.WriteLine("NumberOfNodes\tFileType\tNumberOfTests\tAvgKuhnMunkres");
                sw.WriteLine("MIcroseconds;");
                foreach (var result in results)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgKuhnMunkres}");
                }
            }
        }

        private static void CleanUpTimes()
        {
            bruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
            fordFulkersonTimes = new Dictionary<string, List<long>>();
            kuhnMunkresTimes = new Dictionary<string, List<long>>();
        }

        private static void AddFileTypeToDictionaryIfNeeded(string fileType)
        {
            if (!bruteForceAlgorithmTimes.ContainsKey(fileType))
            {
                bruteForceAlgorithmTimes.Add(fileType, new List<long>());
            }

            if (!fordFulkersonTimes.ContainsKey(fileType))
            {
                fordFulkersonTimes.Add(fileType, new List<long>());
            }

            if (!kuhnMunkresTimes.ContainsKey(fileType))
            {
                kuhnMunkresTimes.Add(fileType, new List<long>());
            }
        }
    }
}
