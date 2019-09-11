namespace GraphMatchings.Console
{
    using CommandLine;

    using GraphMatchings.Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(
                    o =>
                        {
                            ParseInputAndRunAlgorithm(o.InputFile);
                        });
        }

        private static void ParseInputAndRunAlgorithm(string pathToFile)
        {
            var graph = GraphParser.Parse(pathToFile);
            int b = 0;
        }
    }
}