namespace GraphMatchings.GraphsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public static class GraphsGenerator
    {
        private const string SmallGraphsFolder = "SmallGraphs";
        private const string BigGraphsFolder = "BigGraphs";
        private const string GraphsWithGivenEdgesFolder = "GraphsWithGivenEdges";

        private const string Graph6FormatDirectory = "Graph6Format";
        private const string MatrixFormatDirectory = "AdjacencyMatrixFormat";
        private const string CustomFormatDirectory = "MyFormat";

        public static void Generate(string nautyPath)
        {
            PrepareDirectories();

            GenerateAndSaveSmallGraphs(nautyPath);
            Console.WriteLine("Small graphs generated");

            //GenerateAndSaveBigGraphs(nautyPath);
           // Console.WriteLine("Big graphs generated");

            GenerateAndSaveGraphsWithGivenEdges(nautyPath);
            Console.WriteLine("Graphs with given edges generated");
        }

        private static void PrepareDirectories()
        {
            if (Directory.Exists(SmallGraphsFolder))
            {
                Directory.Delete(SmallGraphsFolder, true);
            }

            if (Directory.Exists(BigGraphsFolder))
            {
                Directory.Delete(BigGraphsFolder, true);
            }

            if (Directory.Exists(GraphsWithGivenEdgesFolder))
            {
                Directory.Delete(GraphsWithGivenEdgesFolder, true);
            }

            Directory.CreateDirectory(Path.Combine(SmallGraphsFolder, Graph6FormatDirectory));
            Directory.CreateDirectory(Path.Combine(SmallGraphsFolder, MatrixFormatDirectory));
            Directory.CreateDirectory(Path.Combine(SmallGraphsFolder, CustomFormatDirectory));

            Directory.CreateDirectory(Path.Combine(BigGraphsFolder, Graph6FormatDirectory));
            Directory.CreateDirectory(Path.Combine(BigGraphsFolder, MatrixFormatDirectory));
            Directory.CreateDirectory(Path.Combine(BigGraphsFolder, CustomFormatDirectory));

            Directory.CreateDirectory(Path.Combine(GraphsWithGivenEdgesFolder, Graph6FormatDirectory));
            Directory.CreateDirectory(Path.Combine(GraphsWithGivenEdgesFolder, MatrixFormatDirectory));
            Directory.CreateDirectory(Path.Combine(GraphsWithGivenEdgesFolder, CustomFormatDirectory));
        }

        private static void GenerateAndSaveSmallGraphs(string nautyPath)
        {
            var graph6FormatPath = Path.Combine(SmallGraphsFolder, Graph6FormatDirectory);
            var matrixFormatPath = Path.Combine(SmallGraphsFolder, MatrixFormatDirectory);
            var customFormatPath = Path.Combine(SmallGraphsFolder, CustomFormatDirectory);

            GenerateSmallGraphs(nautyPath, graph6FormatPath);
            FormatChanger.ChangeFromGraph6ToMatrix(nautyPath, graph6FormatPath, matrixFormatPath);
            FormatChanger.ChangeFromMatrixToCustomFormat(nautyPath, matrixFormatPath, customFormatPath);
        }

        private static void GenerateAndSaveBigGraphs(string nautyPath)
        {
            var graph6FormatPath = Path.Combine(BigGraphsFolder, Graph6FormatDirectory);
            var matrixFormatPath = Path.Combine(BigGraphsFolder, MatrixFormatDirectory);
            var customFormatPath = Path.Combine(BigGraphsFolder, CustomFormatDirectory);

            GenerateBigGraphs(nautyPath, graph6FormatPath);
            FormatChanger.ChangeFromGraph6ToMatrix(nautyPath, graph6FormatPath, matrixFormatPath);
            FormatChanger.ChangeFromMatrixToCustomFormat(nautyPath, matrixFormatPath, customFormatPath);
        }

        private static void GenerateAndSaveGraphsWithGivenEdges(string nautyPath)
        {
            var graph6FormatPath = Path.Combine(GraphsWithGivenEdgesFolder, Graph6FormatDirectory);
            var matrixFormatPath = Path.Combine(GraphsWithGivenEdgesFolder, MatrixFormatDirectory);
            var customFormatPath = Path.Combine(GraphsWithGivenEdgesFolder, CustomFormatDirectory);

            GenerateGraphsWithGivenEdges(nautyPath, graph6FormatPath);
            FormatChanger.ChangeFromGraph6ToMatrix(nautyPath, graph6FormatPath, matrixFormatPath);
            FormatChanger.ChangeFromMatrixToCustomFormat(nautyPath, matrixFormatPath, customFormatPath);
        }

        private static void GenerateSmallGraphs(string nautyPath, string graph6FormatOutputPath)
        {
            const int NumberOfNodes = 11;

            for (var partition1Size = 1; partition1Size <= NumberOfNodes; ++partition1Size)
            {
                for (var partition2Size = partition1Size;
                     partition1Size + partition2Size <= NumberOfNodes;
                     ++partition2Size)
                {
                    var outputFilePath = Path.Combine(
                        graph6FormatOutputPath,
                        $"{partition1Size}-{partition2Size}.txt");
                    var command = $"genbg -c {partition1Size} {partition2Size} > {outputFilePath}";

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
        }

        private static void GenerateBigGraphs(string nautyPath, string graph6Path)
        {
            var graphSizes = new List<int> { 20, 50, 100, 500, 1000 };

            foreach (var graphSize in graphSizes)
            {
                var command = $"genrang {graphSize / 2},{graphSize / 2} {10} > {graph6Path}\\{graphSize}.txt";

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

        private static void GenerateGraphsWithGivenEdges(string nautyPath, string graph6Path)
        {
            var graphSize = 100;
            var edgesNumber = new List<int> { 200, 500, 1000, 2000, 3000 };

            foreach (var edgeNumber in edgesNumber)
            {
                var command = $"genrang {graphSize / 2},{graphSize / 2} {10} -e{edgeNumber} > {graph6Path}\\{edgeNumber}.txt";

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
    }
}
