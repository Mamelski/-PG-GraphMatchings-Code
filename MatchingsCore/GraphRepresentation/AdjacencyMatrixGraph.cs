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

        /// <summary>
        /// The nodes.
        /// </summary>
        private List<Node> nodes = new List<Node>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyMatrixGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of nodes.
        /// </param>
        public AdjacencyMatrixGraph(int numberOfNodes)
        {
            this.matrix = new List<List<int>>(new List<int>[numberOfNodes]);

            for (int i = 0; i < numberOfNodes; ++i)
            {
                this.matrix[i] = new List<int>(new int[numberOfNodes]);
                this.nodes.Add(new Node { Id = i });
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index1">
        /// The index 1.
        /// </param>
        /// <param name="index2">
        /// The index 2.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
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