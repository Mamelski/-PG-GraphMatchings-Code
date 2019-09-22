namespace GraphMatchings.Tester
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GraphMatchings.Core;

    public class SmallGraphsUnweighted
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static Dictionary<string, List<long>> BruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> FordFulkersonTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> KuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void Test(string myFormatDirectory)
        {
            CleanupResults();
            TestWithoutDummyFiles(myFormatDirectory);

            CleanupResults();
            TestWitDummyFiles(myFormatDirectory);
            CleanupResults();
        }


        private static void TestWitDummyFiles(string myFormatDirectory)
        {
            var sourceFile = Path.Combine(myFormatDirectory, "1-1-0.txt");
            File.Copy(sourceFile, Path.Combine(myFormatDirectory, "0-0-1.txt"));
            File.Copy(sourceFile, Path.Combine(myFormatDirectory, "0-0-2.txt"));
            File.Copy(sourceFile, Path.Combine(myFormatDirectory, "0-0-3.txt"));

            foreach (var filePath in Directory.GetFiles(myFormatDirectory))
            {
                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";

                var graph = GraphParser.Parse(filePath);

                Run3AlgorithmsAndCheckResults(fileType, graph);
            }

            PrintDummyFilesResult(@"SmallTestsResults\3algs_3DummyFiles.txt");
        }

        private static void TestWithoutDummyFiles(string myFormatDirectory)
        {
            foreach (var filePath in Directory.GetFiles(myFormatDirectory))
            {
                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";

                var graph = GraphParser.Parse(filePath);

                Run3AlgorithmsAndCheckResults(fileType, graph);
            }

            PrintDummyFilesResult(@"SmallTestsResults\3algs_noDummyFiles.txt");
        }

        private static void PrintBig(string outputPath)
        {
            var results = new List<TestResults>();
            foreach (var key in FordFulkersonTimes.Keys)
            {
                var avgFordFulkerson = FordFulkersonTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var avgKuhnMunkres = KuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]) + int.Parse(split[1]),
                                    NumberOfTests = BruteForceAlgorithmTimes[key].Count,
                                    AvgFordFulkerson = Math.Round(avgFordFulkerson, 0),
                                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 0)
                                });
            }

            var sortedResults = results.OrderBy(r => r.NumberOfNodes);

            using (var sw = File.CreateText(outputPath))
            {
                sw.WriteLine("file\tsumOfNodes\tnumberOfTests\tFordFulkerson\tKuhnMunkers");
                foreach (var result in sortedResults)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgFordFulkerson}\t{result.AvgKuhnMunkres}");
                }
            }
        }

        private static void PrintDummyFilesResult(string outputPath)
        {
            var results = new List<TestResults>();
            foreach (var key in BruteForceAlgorithmTimes.Keys)
            {
                var avgBruteForce = BruteForceAlgorithmTimes[key].Average(ticks => (ticks * 1000000) / Stopwatch.Frequency);
                var avgFordFulkerson = FordFulkersonTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var avgKuhnMunkres = KuhnMunkresTimes[key].Average(ticks => ((ticks * 1000000) / Stopwatch.Frequency));
                var split = key.Split('-');

                results.Add(new TestResults
                                {
                                    FileType = key,
                                    NumberOfNodes = int.Parse(split[0]) + int.Parse(split[1]),
                                    NumberOfTests = BruteForceAlgorithmTimes[key].Count,
                                    AvgBruteForce = Math.Round(avgBruteForce, 0),
                                    AvgFordFulkerson = Math.Round(avgFordFulkerson, 0),
                                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 0)
                });
            }

            var sortedResults = results.OrderBy(r => r.NumberOfNodes);

            using (var sw = File.CreateText(outputPath))
            {
                sw.WriteLine("file\tsumOfNodes\tnumberOfTests\tBruteForce\tFordFulkerson\tKuhnMunkers\tMicroseconds");
                foreach (var result in sortedResults)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgBruteForce}\t{result.AvgFordFulkerson}\t{result.AvgKuhnMunkres}");
                }
            }
        }

        private static void Run3AlgorithmsAndCheckResults(string fileType, int[,] graph)
        {
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

            for (var i = 0; i < bruteForceAlgorithmResults.Count; ++i)
            {
                bruteForceAlgorithmResults[i] = TestHelper.SortEdges(bruteForceAlgorithmResults[i]);
            }

            fordFulkersonResult = TestHelper.SortEdges(fordFulkersonResult);
            kuhnMunkersResult = TestHelper.SortEdges(kuhnMunkersResult);

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

        private static void CleanupResults()
        {
            BruteForceAlgorithmTimes = new Dictionary<string, List<long>>();
            FordFulkersonTimes = new Dictionary<string, List<long>>();
            KuhnMunkresTimes = new Dictionary<string, List<long>>();
        }
    }
}