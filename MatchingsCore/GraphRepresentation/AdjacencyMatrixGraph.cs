namespace MatchingsCore.GraphRepresentation
{
    using System.Collections.Generic;

    /// <summary>
    ///     The adjacency list graph.
    /// </summary>
    public class AdjacencyMatrixGraph
    {
        /// <summary>
        /// The nodes.
        /// </summary>
        private readonly List<List<int>> matrix;

        private List<Node> nodes = new List<Node>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyListGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of nodes.
        /// </param>
        public AdjacencyMatrixGraph(int numberOfNodes)
        {
            this.matrix = new List<List<int>>();

            for (int i = 0; i < numberOfNodes; ++i)
            {
                this.matrix[i] = new List<int>(numberOfNodes);
                this.nodes.Add(new Node { Id = i });
            }
        }

        public int this[int index1, int index2]
        {
            get => this.matrix[index1][index2];
            set => this.matrix[index1][index2] = value;
        }

        /// <summary>
        /// The add node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        public int AddNodeAtTheEndAndReturnId()
        {
            var node = new Node { Id = this.nodes.Count };
            
            matrix.Add(new List<int>(this.nodes.Count));
            this.nodes.Add(node);

            for (int i = 0; i < this.nodes.Count; ++i)
            {
                this.matrix[i].Add(0);
            }

            return node.Id;
        }
    }
}