namespace GraphMatchings.Console
{
    using CommandLine;

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o => { ParseInputAndRunAlgorithm(o.InputFile); });
        }

        static void ParseInputAndRunAlgorithm(string pathToFile)
        {
            int b = 0;
        }
    }
}