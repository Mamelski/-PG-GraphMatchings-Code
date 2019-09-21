namespace GraphMatchings.GraphsGenerator
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option('n',
            "nautyFolder",
            Required = true,
            HelpText = @"Path to nauty26r10 folder. Format example: C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\nauty26r10\")]
        public string NautyFolder { get; set; }
    }
}
