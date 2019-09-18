namespace GraphMatchings.GraphGenerator
{
    using CommandLine;

    public class CommandLineOptions
    {
        [Option('g',
            "numberOfGraphs",
            Required = true,
            HelpText = "Number of graphs to generate")]
        public int NumberOfGraphs { get; set; }

        [Option('n',
            "numberOfNodes",
            Required = true,
            HelpText = "Number of nodes to generate in graph")]
        public int NumberOfNodes { get; set; }

        [Option('e',
            "numberOfEdges",
            Required = true,
            HelpText = "Number of edges to generate in graph")]
        public int NumberOfEdges { get; set; }

        [Option('t',
            "graphType",
            Required = true,
            HelpText = "Type of graph to generate. Can be \"W\" or \"N\"")]
        public string GraphType { get; set; }

        [Option('w',
            "maximEdgeWeight",
            Required = false,
            HelpText = "Maximum weight of generated edge")]
        public int MaximumEdgeWeight { get; set; }
    }
}
