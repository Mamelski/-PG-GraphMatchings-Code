namespace MatchingsCore.Graph
{
    using System.Collections.Generic;

    /// <summary>
    ///     The adjacency list graph.
    /// </summary>
    public class AdjacencyListGraph : Graph
    {
        /// <summary>
        ///     The neighbourList.
        /// </summary>
        private readonly List<List<Node>> neighbourList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyListGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of neighbourList.
        /// </param>
        public AdjacencyListGraph(int numberOfNodes)
        {
            this.neighbourList = new List<List<Node>>(numberOfNodes);

            for (int i = 0; i < numberOfNodes; ++i)
            {
                this.neighbourList.Add(new List<Node>());
            }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public List<Node> this[Node node]
        {
            get => this.neighbourList[node.Id];

            set => this.neighbourList[node.Id] = value;
        }
    }
}