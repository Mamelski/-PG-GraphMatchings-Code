
namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using GraphMatchings.Core.Utils;

    public static class HungarianMethod
    {
        private static int[,] matrix;

        private static bool[,] stars;

        private static bool[,] primes;

        private static bool[] isRowCovered;

        private static bool[] isColumnCovered;

        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            Step0(graph);

            stars = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            Step1();
            return null;
        }

        private static void Step0(int[,] graph)
        {
            var colors = BFSGraphColouring.Run(graph);
            var index1 = new List<int>();
            var index2 = new List<int>();

            for (int node = 0; node < colors.Length; ++node)
            {
                if (colors[node] == 1)
                {
                    index1.Add(node);
                }
                else
                {
                    index2.Add(node);
                }
            }

            if (index1.Count > index2.Count)
            {
                var tmp = index1;
                index1 = index2;
                index2 = tmp;
            }

            matrix = new int[index1.Count, index2.Count];

            for (int i = 0; i < index1.Count; ++i)
            {
                for (int j = 0; j < index2.Count; ++j)
                {
                    matrix[i, j] = 0 - graph[index1[i], index2[j]];
                }
            }

            MatrixHelper.PrintMatrix(matrix);
        }

        private static void Step1()
        {
            var minInRow = int.MaxValue;

            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    minInRow = Math.Min(minInRow, matrix[row, column]);
                }

                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    matrix[row, column] -= minInRow;
                }
            }

            MatrixHelper.PrintMatrix(matrix);

            Step2();
        }

        private static void Step2()
        {
            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (matrix[row, column] == 0 && !IsStarInRow(row) && !IsStarInColumn(column))
                    {
                        stars[row, column] = true;
                    }
                }
            }

            MatrixHelper.PrintMatrix(stars);

            Step3();
        }

        private static void Step3()
        {
            var numberOfCoveredColumns = 0;
            for (int column = 0; column < MatrixHelper.ColumnsCount(stars); column++)
            {
                if (IsStarInColumn(column))
                {
                    isColumnCovered[column] = true;
                    ++numberOfCoveredColumns;
                }
            }

            if (numberOfCoveredColumns == MatrixHelper.RowsCount(matrix))
            {
                Step7();
            }
            else
            {
                Step4();
            }
        }

        private static void Step4()
        {
            var rowWith0 = -1;
            var columnWith0 = -1;
            while (IsUncoveredZero(ref rowWith0, ref columnWith0))
            {
                primes[rowWith0, columnWith0] = true;

                if (!IsStarInRow(rowWith0))
                {
                    Step5(rowWith0, columnWith0);
                }
                else
                {
                    var columnWith0Star = 0;
                    for (int column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                    {
                        if (stars[rowWith0, column])
                        {
                            columnWith0Star = column;
                        }
                    }

                    isColumnCovered[columnWith0Star] = false;
                    isRowCovered[rowWith0] = true;
                }
            }

            Step6();
        }

        private static void Step5(int row, int column)
        {
            var path = new List<Tuple<int, int>> { new Tuple<int, int>(row, column) };

            while (true)
            {
                var rowWith0Star = Find0StarInColumn(column);
                if (!rowWith0Star.HasValue)
                {
                    break;
                }

                row = rowWith0Star.Value;
                path.Add(new Tuple<int, int>(row, column));

                var columnWith0Prime = Find0PrimeInRow(row);
                if (columnWith0Prime.HasValue)
                {
                    column = columnWith0Prime.Value;
                    path.Add(new Tuple<int, int>(row, column));
                }
            }

            foreach (var cell in path)
            {
                if (stars[cell.Item1, cell.Item2])
                {
                    stars[cell.Item1, cell.Item2] = false;
                }

                if (primes[cell.Item1, cell.Item2])
                {
                    primes[cell.Item1, cell.Item2] = false;
                    stars[cell.Item1, cell.Item2] = true;
                }
            }

            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];

            Step3();
        }

        private static void Step6()
        {
            var min = matrix[0, 0];
            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    min = Math.Min(min, matrix[row, column]);
                }
            }

            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (isRowCovered[row])
                    {
                        matrix[row, column] += min;
                    }

                    if (!isColumnCovered[column])
                    {
                        matrix[row, column] -= min;
                    }
                }
            }
        }

        private static void Step7()
        {
            MatrixHelper.PrintMatrix(stars);
            int a = 0;

        }

        private static int? Find0StarInColumn(int column)
        {
            for (var row = 0; row < MatrixHelper.ColumnsCount(matrix); ++row)
            {
                if (stars[row, column])
                {
                    return row;
                }
            }

            return null;
        }

        private static int? Find0PrimeInRow(int row)
        {
            for (var column = 0; column < MatrixHelper.RowsCount(matrix); ++column)
            {
                if (primes[row, column])
                {
                    return column;
                }
            }

            return null;
        }

        private static bool IsStarInRow(int row)
        {
            for (int column = 0; column < MatrixHelper.ColumnsCount(stars); column++)
            {
                if (stars[row, column])
                {
                    return true;
                }
            }

            return false;
        }

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

        private static bool IsUncoveredZero(ref int rowWith0, ref int columnWith0)
        {
            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
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
