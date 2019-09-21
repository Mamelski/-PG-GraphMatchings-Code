namespace GraphMatchings.GraphGenerator
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class BigGraphsGenerator
    {
        private const string NautyFolder = @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\nauty26r10\";

        private const string Graph6FormatDirectory = @"BigRandomGraphs\Graph6Format\";

        private const string AdjacencyMatrixFormatDirectory = @"BigRandomGraphs\AdjacencyMatrixFormat\";

        private const string MyFormatDirectory = @"BigRandomGraphs\CustomFormat\";

        public static void Generate()
        {
            GenerateRandomGraphs();
            ChangeRandomGraphsToVisibleFormat();
            ChangeRandomGraphsToCustomFormat();
        }

        private static void GenerateRandomGraphs()
        {
            var graphSizes = new List<int> { 20, 50, 100, 500, 1000 };

            foreach (var graphSize in graphSizes)
            {
                var command = $"genrang {graphSize / 2},{graphSize / 2} {10} > {Graph6FormatDirectory}{graphSize}.txt";

                var cmdProcess = new Process
                                     {
                                         StartInfo =
                                             {
                                                 FileName = "cmd.exe",
                                                 Arguments = $"/C {NautyFolder}{command}",
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

        private static void ChangeRandomGraphsToVisibleFormat()
        {
            foreach (var filePath in Directory.GetFiles(Graph6FormatDirectory))
            {
                var fileName = Path.GetFileName(filePath);
                var command = $"showg -a {Graph6FormatDirectory}{fileName} > {AdjacencyMatrixFormatDirectory}{fileName}";

                var cmdProcess = new Process
                                     {
                                         StartInfo =
                                             {
                                                 FileName = "cmd.exe",
                                                 Arguments = $"/C {NautyFolder}{command}",
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

        private static void ChangeRandomGraphsToCustomFormat()
        {
            foreach (var filePath in Directory.GetFiles(AdjacencyMatrixFormatDirectory))
            {
                var graphNumber = 0;
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var lines = File.ReadAllLines(filePath);

                // Every line in adjcency matrix file
                for (var line = 0; line < lines.Length; ++line)
                {
                    if (string.IsNullOrEmpty(lines[line]))
                    {
                        continue;
                    }

                    var split = lines[line].Split();
                    var numberOfNodesString = split.Last().Replace(".", string.Empty);
                    var numberOfNodes = int.Parse(numberOfNodesString);
                    ++line;

                    var outpuPath = $"{MyFormatDirectory}{fileName}-{graphNumber}.txt";

                    // Single graph in file
                    using (var sw = File.CreateText(outpuPath))
                    {
                        var graphLine = 0;
                        for (; graphLine < numberOfNodes; ++graphLine)
                        {
                            var sb = new StringBuilder();

                            // Single line
                            for (var j = 0; j < numberOfNodes; ++j)
                            {
                                sb.Append($"{lines[line][j]} ");
                            }

                            sb.Remove(sb.Length - 1, 1);
                            sw.WriteLine(sb);

                            ++line;
                        }
                    }
                    ++graphNumber;
                }
            }
        }
    }
}
