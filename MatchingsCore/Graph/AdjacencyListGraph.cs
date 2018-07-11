namespace MatchingsCore.Graph
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The adjacency list graph.
    /// </summary>
    public class AdjacencyListGraph : Graph
    {
        /// <summary>
        /// The nodes.
        /// </summary>
        private List<Node> nodes = new List<Node>();

        public Node this[int i]
        {
            get => this.nodes[i];
            set => this.nodes[i] = value;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override void Add(Node node)
        {
            this.nodes[node.Id] = 
        }
    }
}