namespace GraphMatchings.Tester
{
    using GraphMatchings.Tester.Testers;

    public class Program
    {
        private const string MyFormatDirectory =
            @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\SmallGraphs\MyFormat";

        public static void Main(string[] args)
        {
            //SmallGraphsTester.Test(MyFormatDirectory);
            //SmallWeightedGraphsTester.Test(MyFormatDirectory);
            BigGraphsUnweighted.Test(@"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\BigGraphs\MyFormat");
        }
    }
}