namespace GraphMatchings.Tester
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class TestHelper
    {
        // Sort edges
        public static List<Tuple<int, int>> SortEdges(List<Tuple<int, int>> results)
        {
            var sorted = new List<Tuple<int, int>>();
            foreach (var result in results)
            {
                if (result.Item1 > result.Item2)
                {
                    sorted.Add(new Tuple<int, int>(
                        result.Item2,
                        result.Item1));
                }
                else
                {
                    sorted.Add(result);
                }
            }

            return sorted;
        }

        public static string GetFileType(string filePath)
        {
            var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
            return $"{split[0]}-{split[1]}";
        }
    }
}
