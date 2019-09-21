namespace GraphMatchings.Core
{
    using System.IO;

    public static class GraphParser
    {
        public static int[,] Parse(string pathToFile)
        {
            CheckIfFileExists(pathToFile);

            return ParseFile(pathToFile);
        }

        private static void CheckIfFileExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File \"{path}\" does not exist.");
            }
        }

        private static int[,] ParseFile(string pathToFile)
        {
            var lines = File.ReadAllLines(pathToFile);

            var graph = new int[lines.Length, lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var split = lines[i].Split();
                for (var j = 0; j < split.Length; ++j)
                {
                    var value = int.Parse(split[j]);
                    graph[i, j] = value;
                }
            }

            return graph;
        }
    }
}