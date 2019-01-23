using GraphMatchings.Core.GraphRepresentation;

namespace MatchingsCore.GraphRepresentation
{
    using System.Collections.Generic;

    /// <summary>
    ///     The adjacency list graph.
    /// </summary>
    public class AdjacencyMatrixGraph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyMatrixGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of Nodes.
        /// </param>
        public AdjacencyMatrixGraph(int numberOfNodes)
        {
            this.Matrix = new List<List<int>>(new List<int>[numberOfNodes]);

            for (var i = 0; i < numberOfNodes; ++i)
            {
                this.Matrix[i] = new List<int>(new int[numberOfNodes]);
                this.Nodes.Add(new Node { Id = i });
            }
        }

        /// <summary>
        /// Gets or sets the matrix.
        /// </summary>
        public List<List<int>> Matrix { get; set; }

        /// <summary>
        /// The size.
        /// </summary>
        public int Size => this.Nodes.Count;

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        public List<Node> Nodes { get; set; } = new List<Node>();

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
            get => this.Matrix[index1][index2];
            set => this.Matrix[index1][index2] = value;
        }

        /// <summary>
        /// The add node at the end and return id.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int AddNodeAtTheEndAndReturnId()
        {
            var node = new Node { Id = this.Nodes.Count };

            this.Matrix.Add(new List<int>(new int[this.Nodes.Count]));
            this.Nodes.Add(node);

            for (int i = 0; i < this.Nodes.Count; ++i)
            {
                this.Matrix[i].Add(0);
            }

            return node.Id;
        }
    }
}