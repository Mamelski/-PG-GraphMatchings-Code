namespace GraphMatchings.Core.Utils
{
    public class GraphChecker
    {

        public static bool IsGraphBipartite(int[,] graph)
        {
            return true;
        }

        public static bool IsGraphWeighted(int[,] graph)
        {
            for (var i = 0; i < graph.Length; ++i)
            {
                for (var j = 0; j < graph.Length; ++j)
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
