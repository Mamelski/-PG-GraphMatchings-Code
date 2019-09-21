namespace GraphMatchings.GraphsGenerator
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class FormatChanger
    {
        public static void ChangeFromGraph6ToMatrix(string nautyPath, string inputFolder, string outputFolder)
        {
            foreach (var filePath in Directory.GetFiles(inputFolder))
            {
                var fileName = Path.GetFileName(filePath);
                var command = $"showg -a {inputFolder}\\{fileName} > {outputFolder}\\{fileName}";

                var cmdProcess = new Process
                                     {
                                         StartInfo =
                                             {
                                                 FileName = "cmd.exe",
                                                 Arguments = $"/C {nautyPath}{command}",
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

        public static void ChangeFromMatrixToCustomFormat(string nautyPath, string inputFolder, string outputFolder)
        {
            foreach (var filePath in Directory.GetFiles(inputFolder))
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

                    var outpuPath = $"{outputFolder}\\{fileName}-{graphNumber}.txt";

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
