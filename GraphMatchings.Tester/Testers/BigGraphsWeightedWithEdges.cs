namespace GraphMatchings.Tester.Testers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using GraphMatchings.Core;
    using GraphMatchings.Core.Algorithms;

    public static class BigGraphsWeightedWithEdges
    {
        private static readonly Stopwatch Sw = new Stopwatch();
        private static Dictionary<string, List<long>> fordFulkersonTimes = new Dictionary<string, List<long>>();
        private static Dictionary<string, List<long>> kuhnMunkresTimes = new Dictionary<string, List<long>>();

        public static void Test(string inputDirectory)
        {
            CleanUpTimes();
            TestKuhnMunkres(inputDirectory);
        }

        private static void TestKuhnMunkres(string myFormatDirectory)
        {
            var possibleWeights = new List<Tuple<int, int>>
                                      {
                                          new Tuple<int, int>(1, 100),
                                          new Tuple<int, int>(1, 1000),
                                          new Tuple<int, int>(1, 10000),
                                          new Tuple<int, int>(1000, 10000)
                                      };
            foreach (var pw in possibleWeights)
            {
                foreach (var filePath in Directory.GetFiles(myFormatDirectory))
                {
                    var fileType = Path.GetFileNameWithoutExtension(filePath).Split("-")[0];
                    Console.WriteLine(fileType);

                    var graph = GraphParser.Parse(filePath);
                    TestHelper.AddWeights(ref graph, pw.Item1, pw.Item2);

                    RunFordFulkersonAndKuhnMunkres(fileType, graph);
                }

                var sortedResults = PrepareFordFulkersonAndKuhnMunkresSortedResults();
                PrintFordFulkersonAndKuhnMunkres(
                    sortedResults,
                    $"BigGraphs_Weighted_WithEdges_FordFulkersonKuhnMunkres_weights_{pw.Item1}_{pw.Item2}.txt");
            }
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

        private static List<TestResults> PrepareFordFulkersonAndKuhnMunkresSortedResults()
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
                    NumberOfNodes = int.Parse(split[0]),
                    NumberOfTests = fordFulkersonTimes[key].Count,
                    AvgFordFulkerson = Math.Round(avgFordFulkerson, 2),
                    AvgKuhnMunkres = Math.Round(avgKuhnMunkres, 2)
                });
            }

            return results.OrderBy(r => r.NumberOfNodes).ToList();
        }

        private static void PrintFordFulkersonAndKuhnMunkres(IEnumerable<TestResults> results, string outputFile)
        {
            using (var sw = File.CreateText($"Results\\{outputFile}"))
            {
                sw.WriteLine("NumberOfNodes\tFileType\tNumberOfTests\tAvgFordFulkerson\tAvgKuhnMunkres");
                sw.WriteLine("Microseconds;");
                foreach (var result in results)
                {
                    sw.WriteLine($"{result.NumberOfNodes}\t{result.FileType}\t{result.NumberOfTests}\t{result.AvgFordFulkerson}\t{result.AvgKuhnMunkres}");
                }
            }
        }

        private static void CleanUpTimes()
        {
            fordFulkersonTimes = new Dictionary<string, List<long>>();
            kuhnMunkresTimes = new Dictionary<string, List<long>>();
        }

        private static void AddFileTypeToDictionaryIfNeeded(string fileType)
        {
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
