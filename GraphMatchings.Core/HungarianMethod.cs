
namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;

    using GraphMatchings.Core.Utils;

    public static class HungarianMethod
    {
        private static bool[,] stars;

        private static bool[,] primes;

        private static bool[] isRowCovered;

        private static bool[] isColumnCovered;

        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            var matrix = Step0(graph);

            stars = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            Step1(matrix);
            return null;
        }

        private static int[,] Step0(int[,] graph)
        {
            var colors = BFSGraphColouring.Run(graph);
            var index1 = new List<int>();
            var index2 = new List<int>();

            for (int i = 0; i < colors.GetLength(0); ++i)
            {
                if (colors[i] == 1)
                {
                    index1.Add(i);
                }
                else
                {
                    index2.Add(i);
                }
            }

            if (index1.Count > index2.Count)
            {
                var tmp = index1;
                index1 = index2;
                index2 = tmp;
            }

            var matrix = new int[index1.Count, index2.Count];

            for (int i = 0; i < index1.Count; ++i)
            {
                for (int j = 0; j < index2.Count; ++j)
                {
                    matrix[i, j] = 0 - graph[index1[i], index2[j]];
                }
            }

            MatrixHelper.PrintMatrix(matrix);
            return matrix;
        }

        private static void Step1(int[,] matrix)
        {
            var min = int.MaxValue;
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (var j = 0; j < matrix.GetLength(1); ++j)
                {
                    min = Math.Min(min, matrix[i, j]);
                }

                for (var j = 0; j < matrix.GetLength(1); ++j)
                {
                    matrix[i, j] -= min;
                }
            }

            MatrixHelper.PrintMatrix(matrix);

            Step2(matrix);
        }

        private static void Step2(int[,] matrix)
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

            Step3(matrix);
        }

        private static void Step3(int[,] matrix)
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
                Step7(matrix);
            }
            else
            {
                Step4(matrix);
            }
        }

        private static void Step4(int[,] matrix)
        {
            var rowWith0 = -1;
            var columnWith0 = -1;
            while (IsUncoveredZero(matrix, ref rowWith0, ref columnWith0))
            {
                primes[rowWith0, columnWith0] = true;

                if (!IsStarInRow(rowWith0))
                {
                    Step5(rowWith0, columnWith0, matrix);
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

            Step6(matrix);
        }

        private static void Step6(int[,] matrix)
        {
            throw new NotImplementedException();
        }

        private static void Step5(int row, int column, int[,] matrix)
        {
            throw new NotImplementedException();
        }

        private static void Step7(int[,] matrix)
        {


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

        private static bool IsUncoveredZero(int[,] matrix, ref int rowWith0, ref int columnWith0)
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
