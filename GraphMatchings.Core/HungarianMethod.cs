namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using Utils;

    public static class HungarianMethod
    {
        private static int[,] originalGraph;
        private static int[,] matrix;
        private static bool[,] stars;
        private static bool[,] primes;
        private static bool[] isRowCovered;
        private static bool[] isColumnCovered;

        private static List<int> rowIndexes = new List<int>();
        private static List<int> columnIndexes = new List<int>();

        private static int step;
        private static bool done;

        private static int rowWith0;
        private static int columnWith0;

        // Entry point, runs Kuhn-Munkres algorithm for given graph
        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            originalGraph = graph;

            while (!done)
            {
                RunSteps();
            }

            var matching = Step7();
            return matching;
        }

        // Runs algorithm steps, based on "step" variable
        private static void RunSteps()
        {
            Console.WriteLine($"Step {step}");

            switch (step)
            {
                case 0:
                    Step0();
                    break;
                case 1:
                    Step1();
                    break;
                case 2:
                    Step2();
                    break;
                case 3:
                    Step3();
                    break;
                case 4:
                    Step4();
                    break;
                case 5:
                    Step5();
                    break;
                case 6:
                    Step6();
                    break;
            }
        }

        // Prepare graph
        private static void Step0()
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

            // Number of rows can't be bigger that number of columns
            // Swap if needed
            if (rowIndexes.Count > columnIndexes.Count)
            {
                var tmp = rowIndexes;
                rowIndexes = columnIndexes;
                columnIndexes = tmp;
            }

            // Init matrix and helper structures
            matrix = new int[rowIndexes.Count, columnIndexes.Count];
            stars = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            // Fill matrix with values from original graph
            for (var row = 0; row < rowIndexes.Count; ++row)
            {
                for (var column = 0; column < columnIndexes.Count; ++column)
                {
                    matrix[row, column] = 0 - originalGraph[rowIndexes[row], columnIndexes[column]];
                }
            }

            step = 1;
        }

        private static void Step1()
        {
            var minInRow = int.MaxValue;

            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                // Update min in row
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    minInRow = Math.Min(minInRow, matrix[row, column]);
                }

                // Delete min in row from each row cell
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    matrix[row, column] -= minInRow;
                }
            }

            step = 2;
        }

        private static void Step2()
        {
            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    // Star each 0 in row and column without other starred 0
                    if (matrix[row, column] == 0
                        && !IsStarInRow(row)
                        && !IsStarInColumn(column))
                    {
                        stars[row, column] = true;
                    }
                }
            }

            step = 3;
        }

        private static void Step3()
        {
            var coveredColumns = 0;

            // Cover columns with 0 star and count them
            for (var column = 0; column < MatrixHelper.ColumnsCount(stars); column++)
            {
                if (IsStarInColumn(column))
                {
                    isColumnCovered[column] = true;
                    ++coveredColumns;
                }
            }

            if (coveredColumns == MatrixHelper.RowsCount(matrix))
            {
                // Assigment is done, ready to read result
                done = true;
            }
            else
            {
                step = 4;
            }
        }

        private static void Step4()
        {
            // Find uncovered 0
            while (IsUncoveredZero())
            {
                // Prime uncovered 0
                primes[rowWith0, columnWith0] = true;

                if (!IsStarInRow(rowWith0))
                {
                    step = 5;
                    return;
                }

                // Find 0 star in the same row
                var columnWith0Star = 0;
                for (int column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (stars[rowWith0, column])
                    {
                        columnWith0Star = column;
                    }
                }

                // Cover column with 0 star and uncover row
                isColumnCovered[columnWith0Star] = false;
                isRowCovered[rowWith0] = true;
            }

            step = 6;
        }

        private static void Step5()
        {
            var row = rowWith0;
            var column = columnWith0;

            // Add Z_0 as first path element
            var path = new List<Tuple<int, int>> { new Tuple<int, int>(row, column) };

            while (true)
            {
                // Find 0 star in a column
                var rowWith0Star = Find0StarInColumn(column);
                if (!rowWith0Star.HasValue)
                {
                    break;
                }

                row = rowWith0Star.Value;
                path.Add(new Tuple<int, int>(row, column));

                // Find 0 prime in a row
                var columnWith0Prime = Find0PrimeInRow(row);
                if (columnWith0Prime.HasValue)
                {
                    column = columnWith0Prime.Value;
                    path.Add(new Tuple<int, int>(row, column));
                }
            }

            foreach (var cell in path)
            {
                // Remove star from 0 star in the path
                if (stars[cell.Item1, cell.Item2])
                {
                    stars[cell.Item1, cell.Item2] = false;
                }

                // Change 0 prime to 0 star
                if (primes[cell.Item1, cell.Item2])
                {
                    primes[cell.Item1, cell.Item2] = false;
                    stars[cell.Item1, cell.Item2] = true;
                }
            }

            // Remove primes and uncover all
            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            step = 3;
        }

        private static void Step6()
        {
            var min = int.MaxValue;

            // Find min uncovered value in matrix
            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (!isRowCovered[row] && !isColumnCovered[column])
                    {
                        min = Math.Min(min, matrix[row, column]);
                    }
                }
            }

            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    // Add min to covered rows
                    if (isRowCovered[row])
                    {
                        matrix[row, column] += min;
                    }

                    // Delete min from uncovered columns
                    if (!isColumnCovered[column])
                    {
                        matrix[row, column] -= min;
                    }
                }
            }

            step = 4;
        }

        private static List<Tuple<int, int>> Step7()
        {
            var res = new List<Tuple<int, int>>();

            for (var row = 0; row < MatrixHelper.RowsCount(stars); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(stars); ++column)
                {
                    // Read cells with 0 star in matrix and map them to original graph edges
                    if (stars[row, column])
                    {
                        res.Add(new Tuple<int, int>(rowIndexes[row], columnIndexes[column]));
                    }
                }
            }

            return res;
        }

        // If there is 0 starred in given column returns row of this 0 star
        private static int? Find0StarInColumn(int column)
        {
            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                if (stars[row, column])
                {
                    return row;
                }
            }

            return null;
        }

        // If there is 0 primed in given row returns column of this 0 prime
        private static int? Find0PrimeInRow(int row)
        {
            for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
            {
                if (primes[row, column])
                {
                    return column;
                }
            }

            return null;
        }

        // Checks if there is a starred 0 in a given row
        private static bool IsStarInRow(int row)
        {
            for (var column = 0; column < MatrixHelper.ColumnsCount(stars); column++)
            {
                if (stars[row, column])
                {
                    return true;
                }
            }

            return false;
        }

        // Checks if there is a starred 0 in a given column
        private static bool IsStarInColumn(int column)
        {
            for (int row = 0; row < MatrixHelper.RowsCount(stars); row++)
            {
                if (stars[row, column])
                {
                    return true;
                }
            }

            return false;
        }

        // Checks if there is uncovered zero in the matrix
        // Sets "rowWith0" and "columnWith0" fields
        private static bool IsUncoveredZero()
        {
            for (var row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                if (isRowCovered[row])
                {
                    continue;
                }

                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (isColumnCovered[column])
                    {
                        continue;
                    }

                    if (matrix[row, column] == 0)
                    {
                        rowWith0 = row;
                        columnWith0 = column;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}