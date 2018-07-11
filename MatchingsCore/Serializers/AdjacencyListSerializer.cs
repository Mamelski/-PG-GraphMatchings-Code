namespace MatchingsCore.Serializers
{
    using System;
    using System.IO;
    using System.Linq;

    using MatchingsCore.Graph;

    /// <summary>
    /// The adjacency list serializer.
    /// </summary>
    public class AdjacencyListSerializer
    {
        public Graph Deserialize(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                throw new Exception($"File {pathToFile} does not exist.");
            }

            using (var reader = File.OpenText(pathToFile))
            {
                var numberOfNodesString = reader.ReadLine();
                if (string.IsNullOrEmpty(numberOfNodesString))
                {
                    throw new Exception($"First line in file {pathToFile} is empty. It should contain number of nodes");
                }

                var numberOfNodes = int.Parse(numberOfNodesString);

                for (var i = 0; i < numberOfNodes; ++i)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                    {
                        throw new Exception($"Line {i} in file {pathToFile} is null or empty. Expecting {numberOfNodesString} lines.");
                    }

                    var nodeNumberString = line.First();
                    var neighbours = line.Split(' ').Skip(1);

                   
                }
            }

            return null;

        }
    }
}
