namespace GraphMatchings.Tester
{
    using System.IO;

    using GraphMatchings.Core.Utils;

    public class Program
    {
        private const string MyFormatDirectory =
            @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphGenerator\bin\Debug\netcoreapp2.1\SmallGraphs\MyFormat\";

        public static void Main(string[] args)
        {
            TestSmallGraphs();
        }

        private static void TestSmallGraphs()
        {
            foreach (var filePath in Directory.GetFiles(MyFormatDirectory))
            {
                var split = Path.GetFileNameWithoutExtension(filePath).Split('-');
                var fileType = $"{split[0]}-{split[1]}";

                var graph = GraphParser.Parse(filePath);

                SmallGraphsTester.RunAndCheckResults(fileType, graph);
            }
        }
    }
}