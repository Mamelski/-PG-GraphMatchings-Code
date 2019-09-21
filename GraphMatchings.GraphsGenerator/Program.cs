namespace GraphMatchings.GraphsGenerator
{
    using CommandLine;

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(
                    o =>
                        {
                            GraphsGenerator.Generate(o.NautyFolder);
                        });
        }
    }
}
