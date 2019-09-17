namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;

    using GraphMatchings.Core.Utils;

    public static class BruteForceMatchingAlgorithm
    {
        private static int lastRow;
        private static List<int> rowIndexes;
        private static List<int> columnIndexes;

        private static List<int> availableColumns;

        private static int[,] matrix;
        private static int[,] originalGraph;
        private static Stack<Tuple<int, int>> matchingEdges;

        public static void Run(int[,] graph)
        {
            originalGraph = graph;
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
                    availableColumns.Add(node);
                }
            }

            // Init matrix
            matrix = new int[rowIndexes.Count, columnIndexes.Count];

            // Fill matrix with values from original graph
            for (var row = 0; row < rowIndexes.Count; ++row)
            {
                for (var column = 0; column < columnIndexes.Count; ++column)
                {
                    matrix[row, column] = 0 - originalGraph[rowIndexes[row], columnIndexes[column]];
                }
            }
        }

        private static void RunStep(int row)
        {
            if (row == lastRow)
            {
                // TODO zapisz wynik
                return;
            }

            // Find all possible column values for given row
            foreach (var column in availableColumns)
            {
                // Add edge to matching, column is not available
                matchingEdges.Push(new Tuple<int, int>(row, column));
                availableColumns.Remove(column);

                RunStep(row + 1);

                // Remove edge from matching, column is now available
                matchingEdges.Pop();
                availableColumns.Add(column);
            }
        }
    }
}