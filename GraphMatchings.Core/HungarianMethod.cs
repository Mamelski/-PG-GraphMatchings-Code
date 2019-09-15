
namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public static class HungarianMethod
    {
        public static List<Tuple<int, int>> Run(int[,] graph)
        {
            var matrix = Step0(graph);
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

            if(index1.Count > )

            throw new NotImplementedException();
        }
    }
}
