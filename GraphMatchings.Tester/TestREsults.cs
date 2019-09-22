namespace GraphMatchings.Tester
{
    public class TestResults
    {
        public string FileType { get; set; }

        public int NumberOfNodes { get; set; }

        public double AvgBruteForce { get; set; }

        public double AvgFordFulkerson { get; set; }

        public double AvgKuhnMunkres { get; set; }

        public int NumberOfTests { get; set; }
    }
}
