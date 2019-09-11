namespace GraphMatchings.Core.Utils
{
    public class WeightsChecker
    {
        public static bool IsGraphWeighted(int[,] graph)
        {
            for (var i = 0; i < graph.GetLength(0); ++i)
            {
                for (var j = 0; j < graph.GetLength(1); ++j)
                {
                    if (graph[i, j] != 0 && graph[i, j] != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
