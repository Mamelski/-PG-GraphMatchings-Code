namespace GraphMatchings.Tester
{
    public class Program
    {
        private const string MyFormatDirectory =
            @"C:\Users\Jakub\Documents\GitHub\-PG-GraphMatchings-Code\GraphMatchings.GraphGenerator\bin\Debug\netcoreapp2.1\SmallGraphs\MyFormat\";

        public static void Main(string[] args)
        {
            SmallGraphsTester.TestSmallGraphs(MyFormatDirectory);
        }
    }
}