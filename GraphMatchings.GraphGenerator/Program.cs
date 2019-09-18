namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CommandLine;

    public class Program
    {

        private static bool isWeighted;

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(
                    o =>
                        {
                            ParseInputAndRunAlgorithm(o);
                        });
        }

        private static void ParseInputAndRunAlgorithm(CommandLineOptions options)
        {
            if (char.ToLower(options.GraphType[0]) == 'w')
            {
                isWeighted = true;
            }
            else
            {
                if (char.ToLower(options.GraphType[0]) == 'n')
                {
                    isWeighted = false;
                }
                else
                {
                    throw new Exception($"First line in file should contain \"W\" or \"N\" to determine if given graph is weighted or not. Current line is {options.GraphType}");
                }
            }

            var res = new List<int[,]>();
            for (int i = 0; i < options.NumberOfGraphs; ++i)
            {
                var x = GenerateGraph(options.NumberOfNodes, options.NumberOfEdges);
                res.Add(x);
            }

            SaveGeneratedGraphs(res, options.NumberOfNodes, options.NumberOfEdges, true);
        }

        private static int[,] GenerateGraph(int numberOfNodes, int numberOfEdges)
        {
            var random = new Random();
            var rowIndexes = new List<int>();
            var columnIndexes = new List<int>();

            // Split nodes into two partitions
            for (var node = 0; node < numberOfNodes / 2; node++)
            {
                rowIndexes.Add(node);
            }

            for (var node = numberOfNodes / 2; node < numberOfNodes; node++)
            {
                columnIndexes.Add(node);
            }

            var matrix = new int[numberOfNodes, numberOfNodes];

            // Make graph connected
            var notConnectedNodes = rowIndexes.Select(c => c).ToList();

            foreach (var node in columnIndexes)
            {
                if (notConnectedNodes.Count == 0)
                {
                    notConnectedNodes = rowIndexes.Select(c => c).ToList();
                }

                var neighbour = random.Next(notConnectedNodes.Count - 1);

                var weight = isWeighted ? random.Next(10000) : 1;


                matrix[notConnectedNodes[neighbour], node] = weight;
                matrix[node, notConnectedNodes[neighbour]] = weight;

                notConnectedNodes.Remove(notConnectedNodes[neighbour]);
            }

            var numberOfEdgesToAdd = numberOfEdges - columnIndexes.Count;
            for (var i = 0; i < numberOfEdgesToAdd; ++i)
            {
                var isHit = false;

                while (!isHit)
                {
                    var node1 = random.Next(columnIndexes.Count);
                    var node2 = random.Next(rowIndexes.Count);

                    if (matrix[columnIndexes[node1], rowIndexes[node2]] != 0)
                    {
                        continue;
                    }

                    var weight = isWeighted ? random.Next(10000) : 1;

                    matrix[columnIndexes[node1], rowIndexes[node2]] = weight;
                    matrix[rowIndexes[node2], columnIndexes[node1]] = weight;
                    isHit = true;
                }
            }

            return matrix;
        }

        private static void SaveGeneratedGraphs(List<int[,]> graphs, int numberOfNodes, int numberOfEdges, bool isWeighted)
        {
            var typeString = isWeighted ? "W" : "N";

            var folder = Directory.CreateDirectory(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));

            for (var g = 0; g < graphs.Count; g++)
            {
                var fileName = $"{numberOfNodes}-{numberOfEdges}-{typeString}-{g}.txt";
                var path = Path.Combine(folder.Name, fileName);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(numberOfNodes);
                    for (int i = 0; i < graphs[g].GetLength(0); i++)
                    {
                        var sb = new StringBuilder();
                        for (int j = 0; j < graphs[g].GetLength(1); j++)
                        {
                            sb.Append($"{graphs[g][i, j]} ");
                        }

                        sw.WriteLine(sb);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("OK");
            Console.WriteLine($"Graphs saven in folder {folder.FullName}");
            Console.WriteLine();

            int a = 0;
        }
    }
}
