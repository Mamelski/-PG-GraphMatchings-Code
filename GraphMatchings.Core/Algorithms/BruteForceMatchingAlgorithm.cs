namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class BruteForceMatchingAlgorithm
    {
        private static int[,] matrix;
        private static int[,] originalGraph;

        private static List<int> rowIndexes = new List<int>();
        private static List<int> columnIndexes = new List<int>();

        private static bool[] isColumnTaken;
        private static int lastRow;

        private static Stack<Tuple<int, int>> matchingEdges = new Stack<Tuple<int, int>>();
        private static List<List<Tuple<int, int>>> maksimumMatchings = new List<List<Tuple<int, int>>>();
        private static List<List<Tuple<int, int>>> resultMatchings = new List<List<Tuple<int, int>>>();
        private static int bestScore;

        public static List<List<Tuple<int, int>>> Run(int[,] graph)
        {
            originalGraph = graph;
            TransformGraph();
            RunStep(0);
            CleanUpMatchings();

            return resultMatchings;
        }

        private static void CleanUpMatchings()
        {
            foreach (var matching in maksimumMatchings)
            {
                if (matching.All(e => originalGraph[rowIndexes[e.Item1], columnIndexes[e.Item2]] != 0))
                {
                    var resultMatching = new List<Tuple<int, int>>();

                    foreach (var edge in matching)
                    {
                        var v = rowIndexes[edge.Item1];
                        var w = columnIndexes[edge.Item2];
                        resultMatching.Add(new Tuple<int, int>(v, w));
                    }

                    resultMatchings.Add(resultMatching);
                }
            }
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

            // We need more rows than columns
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

            lastRow = rowIndexes.Count - 1;
        }

        private static void RunStep(int row)
        {
            if (row == lastRow)
            {
                // Not taking this row
                CheckScore();

                for (var column = 0; column < columnIndexes.Count; ++column)
                {
                    if (isColumnTaken[column])
                    {
                        continue;
                    }

                    // Add edge to matching
                    matchingEdges.Push(new Tuple<int, int>(row, column));
                    CheckScore();

                    // Add edge to matching
                    matchingEdges.Pop();
                }

                return;
            }

            // Simulate not taking this row
            RunStep(row + 1);

            // Find all possible column values for given row
            for (var column = 0; column < columnIndexes.Count; ++column)
            {
                if (isColumnTaken[column])
                {
                    continue;
                }

                // Add edge to matching, column is not available
                matchingEdges.Push(new Tuple<int, int>(row, column));
                isColumnTaken[column] = true;

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