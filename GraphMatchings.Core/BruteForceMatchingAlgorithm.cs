namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    using GraphMatchings.Core.Utils;

    public static class BruteForceMatchingAlgorithm
    {
        private static int lastRow;
        private static List<int> rowIndexes = new List<int>();
        private static List<int> columnIndexes = new List<int>();
        private static bool[] isColumnTaken;

        private static bool[,] isHit;

        private static int[,] matrix;
        private static int[,] originalGraph;
        private static Stack<Tuple<int, int>> matchingEdges = new Stack<Tuple<int, int>>();

        private static List<List<Tuple<int, int>>> maksimumMatchings = new List<List<Tuple<int, int>>>();
        private static int bestScore;

        public static void Run(int[,] graph)
        {
            originalGraph = graph;
            TransformGraph();

            isHit = new bool[rowIndexes.Count, columnIndexes.Count];

            // MatrixHelper.PrintMatrix(originalGraph);
            MatrixHelper.PrintMatrix(matrix);
            RunStep(0);
        }

        // Transformation similar to the one in HungarianMethod
        private static void TransformGraph()
        {
            // Color graph using BFS
            var colors = GraphColoringBfs.Run(originalGraph);

            // Nodes with color 1 are row indexes in new matrix
            // Nodes with color 2 are column indexes in new matrix
            for (var node = 0; node < colors.Length; ++node)
            {
                if (colors[node] == 1)
                {
                    rowIndexes.Add(node);
                }

                if (colors[node] == 2)
                {
                    columnIndexes.Add(node);
                }
            }

            // Wiecej rows chcemy
            // Swap if needed
            if (rowIndexes.Count < columnIndexes.Count)
            {
                var tmp = rowIndexes;
                rowIndexes = columnIndexes;
                columnIndexes = tmp;
            }

            // Init matrix
            matrix = new int[rowIndexes.Count, columnIndexes.Count];
            isColumnTaken = new bool[columnIndexes.Count];

            // Fill matrix with values from original graph
            for (var row = 0; row < rowIndexes.Count; ++row)
            {
                for (var column = 0; column < columnIndexes.Count; ++column)
                {
                    matrix[row, column] = originalGraph[rowIndexes[row], columnIndexes[column]];
                }
            }

            lastRow = rowIndexes[rowIndexes.Count - 1];
        }

        private static void RunStep(int row)
        {
            if (row == lastRow)
            {
                // Not taking this row
                CheckScore();

                for (var column = 0; column < columnIndexes.Count; ++column)
                {
                    //Console.WriteLine($"{row} {column}");
                    if (isColumnTaken[column])
                    {
                        continue;
                    }

                    // Add edge to matching, column is not available
                    matchingEdges.Push(new Tuple<int, int>(row, column));

                    isHit[row, column] = true;

                    CheckScore();

                    // Remove edge from matching, column is now available
                    matchingEdges.Pop();
                }

                return;
            }

            // Simulate not taking this row
            RunStep(row + 1);

            // Find all possible column values for given row
            for (var column = 0; column < columnIndexes.Count; ++column)
            {
                Console.WriteLine($"{row} {column}");
                if (isColumnTaken[column])
                {
                    continue;
                }

                // Add edge to matching, column is not available
                matchingEdges.Push(new Tuple<int, int>(row, column));
                isColumnTaken[column] = true;

                isHit[row, column] = true;
                RunStep(row + 1);

                // Remove edge from matching, column is now available
                matchingEdges.Pop();
                isColumnTaken[column] = false;
            }
        }

        private static void CheckScore()
        {
            var score = 0;
            foreach (var edge in matchingEdges)
            {
                score += originalGraph[rowIndexes[edge.Item1], columnIndexes[edge.Item2]];
            }

            if (score == bestScore)
            {
                var bestM = matchingEdges.Select(c => c).ToList();
                maksimumMatchings.Add(bestM);
            }

            if (score > bestScore)
            {
                bestScore = score;
                maksimumMatchings.Clear();
                var bestM = matchingEdges.Select(c => c).ToList();
                maksimumMatchings.Add(bestM);
            }
        }
    }
}