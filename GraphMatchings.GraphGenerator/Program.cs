namespace GraphMatchings.GraphGenerator
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Program
    {
        private const string NautyFolder = @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\nauty26r10\";

        private const string Graph6FormatDirectory = @"SmallGraphs\Graph6Format\";

        private const string AdjacencyMatrixFormatDirectory = @"SmallGraphs\AdjacencyMatrixFormat\";

        private const string MyFormatDirectory = @"SmallGraphs\MyFormat\";

        public static void Main(string[] args)
        {
           GenerateAllBipartiteConnectedGraphsWithMax10Nodes();
           ChangeGeneratedBipartiteConnectedGraphsWithMax10NodesToVisibleFormat();
           ChangeVisibleFormatToCustom();
        }

        private static void GenerateAllBipartiteConnectedGraphsWithMax10Nodes()
        {
            for (var i = 1; i <= 10; ++i)
            {
                for (var j = i; i + j <= 10; ++j)
                {
                    var command = $"genbg -c {i} {j} > {Graph6FormatDirectory}{i}-{j}.txt";

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
        }

        private static void ChangeGeneratedBipartiteConnectedGraphsWithMax10NodesToVisibleFormat()
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

        private static void ChangeVisibleFormatToCustom()
        {
            foreach (var filePath in Directory.GetFiles(AdjacencyMatrixFormatDirectory))
            {
                var graphNumber = 0;
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                string[] lines = File.ReadAllLines(filePath);

                // Every line in adjcency matrix file
                for (var line = 0; line < lines.Length; ++line)
                {
                    if (!string.IsNullOrEmpty(lines[line]))
                    {
                        var split = lines[line].Split();
                        var numberOfNodesString = split.Last().Replace(".", string.Empty);
                        var numberOfNodes = int.Parse(numberOfNodesString);
                        ++line;

                        var outpuPath = $"{MyFormatDirectory}{fileName}-{graphNumber}.txt";

                        // Single graph in file
                        using (var sw = File.CreateText(outpuPath))
                        {
                            int graphLine = 0;
                            for (; graphLine < numberOfNodes; ++graphLine)
                            {
                                var sb = new StringBuilder();

                                // Single line
                                for (int j = 0; j < numberOfNodes; ++j)
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
}
