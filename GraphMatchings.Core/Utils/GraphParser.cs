namespace GraphMatchings.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class GraphParser
    {
        public static int[,] Parse(string pathToFile)
        {
            CheckIfFileExists(pathToFile);

            int[,] graph;

            using (var reader = File.OpenText(pathToFile))
            {
                var numberOfNodes = ParseNumberOfNodesFromFile(reader, pathToFile);

                graph = new int[numberOfNodes, numberOfNodes];

                //TODO nody po kolei
                for (var i = 0; i < numberOfNodes; ++i)
                {
                    var line = GetLineFromFile(reader, i, numberOfNodes, pathToFile);

                    var nodeId = GetNodeIdFromLine(line, pathToFile);

                    var neighbours = GetNeighboursFromLine(line, pathToFile);

                    foreach (var neighbourId in neighbours)
                    {
                        graph[nodeId, neighbourId] = 1;
                        graph[neighbourId, nodeId] = 1;
                    }
                }
            }

            return graph;
        }

        private static void CheckIfFileExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File \"{path}\" does not exist.");
            }
        }

        private static int ParseNumberOfNodesFromFile(TextReader reader, string path)
        {
            var numberOfNodesLine = reader.ReadLine();
            if (string.IsNullOrEmpty(numberOfNodesLine))
            {
                throw new Exception($"First line in file \"{path}\" is empty. It should contain number of Nodes");
            }

            var isParseSuccessfull = int.TryParse(numberOfNodesLine, out var numberOfNodes);

            if (!isParseSuccessfull)
            {
                throw new Exception($"Could not parse first line \"{numberOfNodesLine}\" from file \"{path}\" to int. This line should represent number of Nodes.");
            }

            return numberOfNodes;
        }

        private static string GetLineFromFile(StreamReader reader, int i, int numberOfNodes, string pathToFile)
        {
            var line = reader.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                throw new Exception($"Line {i} in file {pathToFile} is null or empty. Expecting {numberOfNodes} lines.");
            }

            return line;
        }

        private static int GetNodeIdFromLine(string line, string path)
        {
            var nodes = line.Split(' ');
            var nodeNumberString = nodes.First();

            var isNodeNumberCorrect = int.TryParse(nodeNumberString, out var nodeId);

            if (!isNodeNumberCorrect)
            {
                throw new Exception($"Could not parse first line \"{line}\" from file \"{path}\" to int. This line should represent number of Nodes.");
            }

            return nodeId;
        }

        private static List<int> GetNeighboursFromLine(string line, string path)
        {
            var splitedLine = line.Split(' ');
            var neighbours = new List<int>();

            // We are starting from 1 because first number in the line is the node and neighbours start from 1.
            for (int i = 1; i < splitedLine.Length; ++i)
            {
                var isParseSuccessfull = int.TryParse(splitedLine[i], out var nodeId);

                if (!isParseSuccessfull)
                {
                    throw new Exception($"Could not parse character \"{splitedLine[i]}\" from file \"{path}\" to int. This string should represent node.");
                }

                neighbours.Add(nodeId);
            }

            return neighbours;
        }
    }
}