namespace GraphMatchings.Core.Utils
{
    using System;

    public static class MatrixHelper
    {
        public static void PrintMatrix<T>(T[,] matrix)
        {
            for (var row = 0; row < RowsCount(matrix); ++row)
            {
                for (var column = 0; column < ColumnsCount(matrix); ++column)
                {
                    Console.Write($"{matrix[row, column]} ");
                }

                Console.WriteLine();
            }
        }

        public static int RowsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(0);
        }

        public static int ColumnsCount<T>(T[,] matrix)
        {
            return matrix.GetLength(1);
        }
    }
}
