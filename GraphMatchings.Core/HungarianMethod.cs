
namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using GraphMatchings.Core.Utils;

    public static class HungarianMethod
    {
        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            var matrix = Step0(graph);

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
            throw new NotImplementedException();
        }
    }
}
