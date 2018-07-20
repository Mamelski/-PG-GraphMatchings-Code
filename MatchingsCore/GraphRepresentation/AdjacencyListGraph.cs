namespace MatchingsCore.GraphRepresentation
{
    using System.Collections.Generic;

    /// <summary>
    ///     The adjacency list graph.
    /// </summary>
    public class AdjacencyListGraph : Graph
    {
        /// <summary>
        /// The nodes.
        /// </summary>
        private readonly List<Node> nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyListGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of nodes.
        /// </param>
        public AdjacencyListGraph(int numberOfNodes)
        {
            this.Size = numberOfNodes;
            this.nodes = new List<Node>(numberOfNodes);

            for (int i = 0; i < numberOfNodes; ++i)
            {
                this.nodes.Add(new Node { Id = i });
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="Node"/>.
        /// </returns>
        public override Node this[int index]
        {
            get => this.nodes[index];
            set => this.nodes[index] = value;
        }
    }
}