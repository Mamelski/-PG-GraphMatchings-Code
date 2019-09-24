namespace GraphMatchings.Tester
{
    using System.Diagnostics;

    using GraphMatchings.Tester.Testers;

    public class Program
    {
        private const string MyFormatDirectory =
            @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\SmallGraphs\MyFormat";

        public static void Main(string[] args)
        {
            var sw = new Stopwatch();
            var d = Stopwatch.IsHighResolution;
            var a = 0;

            //SmallGraphsUnweighted.Test(MyFormatDirectory);
            //SmallWeightedGraphsTester.Test(MyFormatDirectory);

            // BigGraphsUnweighted.Test(@"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\BigGraphs\MyFormat");
            // BigGraphsWeighted.Test(@"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\BigGraphs\MyFormat");

            //BigGraphsUnweightedWithEdges.Test(@"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\GraphsWithGivenEdges\MyFormat");

           // BigGraphsWeightedWithEdges.Test(@"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphsGenerator\bin\Debug\netcoreapp2.1\GraphsWithGivenEdges\MyFormat");
        }
    }
}