namespace GraphMatchings.Tester.Testers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GraphMatchings.Core;

    public static class BigGraphsUnweighted
    {
        private static readonly Stopwatch Sw = new Stopwatch();
        private static Dictionary<string, List<long>> bruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> fordFulkersonTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> kuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void Test(string inputDirectory)
        {
            CleanUpTimes();
            TestBruteForce(inputDirectory);

            CleanUpTimes();
            TestFordFulkersonAndKuhnMunkres(inputDirectory);
        }

        private static void TestBruteForce(string myFormatDirectory)
        {
            var filesToTest = new List<string>
                                  {
                                      Path.Combine(myFormatDirectory, "20-0.txt"),
                                      Path.Combine(myFormatDirectory, "20-1.txt"),
                                      Path.Combine(myFormatDirectory, "20-2.txt"),
                                  };

            foreach (var filePath in filesToTest)
            {
                var fileType = TestHelper.GetFileType(filePath);
                Console.WriteLine(fileType);

                var graph = GraphParser.Parse(filePath);

                RunBruteForce(fileType, graph);
            }

            var sortedResults = PrepareUnweightedBruteForceSortedResults();
            PrintUnweightedBruteForces(sortedResults, @"BigGraphs_Unweighted_BruteForce.txt");
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

        private static List<TestResults> PrepareUnweightedBruteForceSortedResults()
        {
            var results = new List<TestResults>();
            foreach (var key in fordFulkersonTimes.Keys)
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

        private static void PrintUnweightedBruteForces(IEnumerable<TestResults> results, string outputFile)
        {
            using (var sw = File.CreateText($"Results\\{outputFile}"))
            {
                sw.WriteLine("NumberOfNodes\tFileType\tNumberOfTests\tAvgBruteForce");
                sw.WriteLine("Microseconds;");
                foreach (var result in results)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgBruteForce}");
                }
            }
        }

        private static void TestFordFulkersonAndKuhnMunkres(string myFormatDirectory)
        {
            foreach (var filePath in Directory.GetFiles(myFormatDirectory))
            {
                var fileType = TestHelper.GetFileType(filePath);
                Console.WriteLine(fileType);

                var graph = GraphParser.Parse(filePath);

                RunFordFulkersonAndKuhnMunkres(fileType, graph);
            }

            var sortedResults = PrepareUnweightedFordFulkersonAndKuhnMunkresSortedResults();
            PrintUnweightedFordFulkersonAndKuhnMunkres(sortedResults, @"BigGraphs_Unweighted_FordFulkersonKuhnMunkres.txt");
        }

        private static void RunFordFulkersonAndKuhnMunkres(string fileType, int[,] graph)
        {
            AddFileTypeToDictionaryIfNeeded(fileType);

            Sw.Reset();
            Sw.Start();
            FordFulkersonMethod.Run(graph);
            Sw.Stop();

            fordFulkersonTimes[fileType].Add(Sw.ElapsedTicks);

            Sw.Reset();
            Sw.Start();
            HungarianMethod.Run(graph);
            Sw.Stop();

            kuhnMunkresTimes[fileType].Add(Sw.ElapsedTicks);
        }

        private static List<TestResults> PrepareUnweightedFordFulkersonAndKuhnMunkresSortedResults()
        {
            var results = new List<TestResults>();
            foreach (var key in fordFulkersonTimes.Keys)
            {
                var avgFordFulkerson = fordFulkersonTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var avgKuhnMunkres = kuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]) + int.Parse(split[1]),
                                    NumberOfTests = bruteForceAlgorithmTimes[key].Count,
                                    AvgFordFulkerson = Math.Round(avgFordFulkerson, 0),
                                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 0)
                                });
            }

            return results.OrderBy(r => r.NumberOfNodes).ToList();
        }

        private static void PrintUnweightedFordFulkersonAndKuhnMunkres(IEnumerable<TestResults> results, string outputFile)
        {
            using (var sw = File.CreateText($"Results\\{outputFile}"))
            {
                sw.WriteLine("NumberOfNodes\tFileType\tNumberOfTests\tAvgFordFulkerson\tAvgKuhnMunkres");
                sw.WriteLine("Seconds;");
                foreach (var result in results)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgFordFulkerson}\t{result.AvgKuhnMunkres}");
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
