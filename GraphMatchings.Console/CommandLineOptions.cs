namespace GraphMatchings.Console
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option('f',
            "fileName",
            Required = true,
            HelpText = "Input file containing connected graph. "
                       + "First line should be the number of nodes (n). "
                       + "The next following n lines should contain node number and then all its neighbour nodes numbers, all separated by spaces.")]
        public string InputFile { get; set; }
    }
}
