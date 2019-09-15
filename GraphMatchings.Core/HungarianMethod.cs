
namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;

    using GraphMatchings.Core.Utils;

    public static class HungarianMethod
    {
        private static int[,] originalGraph;

        private static int[,] matrix;

        private static bool[,] stars;

        private static bool[,] primes;

        private static bool[] isRowCovered;

        private static bool[] isColumnCovered;

        private static List<int> index1 = new List<int>();
        private static List<int> index2 = new List<int>();

        public static int step;
        private static int rowWith0;
        private static int columnWith0;

        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            originalGraph = graph;

            bool done = false;
            while (!done)
            {
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
                    case 7:
                        Step7();
                        done = true;
                        break;
                }
            }

            return Step7();
        }

        private static void Step0()
        {
            Console.WriteLine("Step 0");
            var colors = BFSGraphColouring.Run(originalGraph);

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
                    matrix[i, j] = 0 - originalGraph[index1[i], index2[j]];
                }
            }

            stars = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            primes = new bool[MatrixHelper.RowsCount(matrix), MatrixHelper.ColumnsCount(matrix)];
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            MatrixHelper.PrintMatrix(matrix);
            step = 1;
        }

        private static void Step1()
        {
            Console.WriteLine("Step 1");
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

            step = 2;
        }

        private static void Step2()
        {
            Console.WriteLine("Step 2");
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

            step = 3;
        }

        private static void Step3()
        {
            Console.WriteLine("Step 3");

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
                step = 7;
            }
            else
            {
                step = 4;
            }
        }

        private static void Step4()
        {
            Console.WriteLine("Step 4");

            while (IsUncoveredZero())
            {
                primes[rowWith0, columnWith0] = true;

                if (!IsStarInRow(rowWith0))
                {
                    step = 5;
                    return;
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

            step = 6;
        }

        private static void Step5()
        {
            int row = rowWith0;
            int column = columnWith0;
            Console.WriteLine("Step 5");
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
            isRowCovered = new bool[MatrixHelper.RowsCount(matrix)];
            isColumnCovered = new bool[MatrixHelper.ColumnsCount(matrix)];

            MatrixHelper.PrintMatrix(stars);

            step = 3;
        }

        private static void Step6()
        {
            Console.WriteLine("Step 6");
            var min = int.MaxValue;
            for (int row = 0; row < MatrixHelper.RowsCount(matrix); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(matrix); ++column)
                {
                    if (!isRowCovered[row] && !isColumnCovered[column])
                    {
                        min = Math.Min(min, matrix[row, column]);
                    }
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

            step = 4;
        }

        private static List<Tuple<int, int>> Step7()
        {
            Console.WriteLine("Step 7");
            MatrixHelper.PrintMatrix(stars);

            var res = new List<Tuple<int, int>>();

            for (int row = 0; row < MatrixHelper.RowsCount(stars); ++row)
            {
                for (var column = 0; column < MatrixHelper.ColumnsCount(stars); ++column)
                {
                    if (stars[row, column])
                    {
                        res.Add(new Tuple<int, int>(index1[row], index2[column]));
                    }
                }
            }

            int a = 0;
            return res;

        }

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

        private static bool IsUncoveredZero()
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
