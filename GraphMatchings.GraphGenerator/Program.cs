namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CommandLine;

    public class Program
    {

        private static bool isWeighted;

        private static string nautyFolder = @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\nauty26r10\";

        private static string graph6FormatDirectory = @"Bigraphs\Graph6Format\";

        private static string adjacencyMatrixFormatDirectory = @"Bigraphs\AdjacencyMatrixFormat\";

        public static void Main(string[] args)
        {
            GenerateAllBipartiteConnectedGraphsWithMax10Nodes();
            ChangeGeneratedBipartiteConnectedGraphsWithMax10NodesToVisibleFormat();
        }

        private static void GenerateAllBipartiteConnectedGraphsWithMax10Nodes()
        {
            for (var i = 1; i <= 10; ++i)
            {
                for (var j = i; i + j <= 10; ++j)
                {
                    var command = $"genbg -c {i} {j} > {graph6FormatDirectory}{i}-{j}.txt";

                    var cmdProcess = new Process
                    {
                        StartInfo =
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/C {nautyFolder}{command}",
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true,
                            UseShellExecute = false
                        }
                    };

                    cmdProcess.Start();

                    cmdProcess.StandardInput.Flush();
                    cmdProcess.StandardInput.Close();
                    cmdProcess.WaitForExit();
                }
            }
        }

        private static void ChangeGeneratedBipartiteConnectedGraphsWithMax10NodesToVisibleFormat()
        {
            foreach (var filePath in Directory.GetFiles(graph6FormatDirectory))
            {
                var fileName = Path.GetFileName(filePath);
                var command =
                    $"showg -a {graph6FormatDirectory}{fileName} > {adjacencyMatrixFormatDirectory}{fileName}\"";

                var cmdProcess = new Process
                                     {
                                         StartInfo =
                                             {
                                                 FileName = "cmd.exe",
                                                 Arguments = $"/C {nautyFolder}{command}",
                                                 RedirectStandardInput = true,
                                                 RedirectStandardOutput = true,
                                                 CreateNoWindow = true,
                                                 UseShellExecute = false
                                             }
                                     };

                cmdProcess.Start();

                cmdProcess.StandardInput.Flush();
                cmdProcess.StandardInput.Close();
                cmdProcess.WaitForExit();
            }
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
