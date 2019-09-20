namespace GraphMatchings.GraphGenerator
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class Program
    {
        private static string nautyFolder = @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\nauty26r10\";

        private static string graph6FormatDirectory = @"SmallGraphs\Graph6Format\";

        private static string adjacencyMatrixFormatDirectory = @"SmallGraphs\AdjacencyMatrixFormat\";
        private static string myFormatDirectory = @"MyFormat\AdjacencyMatrixFormat\";
        public static void Main(string[] args)
        {
           // GenerateAllBipartiteConnectedGraphsWithMax10Nodes();
           //ChangeGeneratedBipartiteConnectedGraphsWithMax10NodesToVisibleFormat();
            ChangeVisibleFormatToCustom();
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

        private static void ChangeVisibleFormatToCustom()
        {
            foreach (var filePath in Directory.GetFiles(adjacencyMatrixFormatDirectory))
            {
                var graphNumber = 0;
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var outpuPath = $"{myFormatDirectory}{fileName}-{graphNumber}.txt";
                string[] lines = File.ReadAllLines(filePath);

                for (var i = 0; i < lines.Length; ++i)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        var numberOfNodesString = lines[i][lines[i].Length - 2].ToString();
                        var numberOfNodes = int.Parse(numberOfNodesString);
                       // using (StreamWriter sw = File.CreateText(path))
                        {

                        }

                        int c= 0;
                    }
                }

            }

            int a = 0;
        }
    }
}
