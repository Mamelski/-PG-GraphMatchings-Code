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
        private readonly List<List<int>> neighbourList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyListGraph"/> class.
        /// </summary>
        /// <param name="numberOfNodes">
        /// The number of neighbourList.
        /// </param>
        public AdjacencyListGraph(int numberOfNodes)
        {
            this.neighbourList = new List<List<int>>(numberOfNodes);

            for (int i = 0; i < numberOfNodes; ++i)
            {
                this.neighbourList.Add(new List<int>());
            }
        }

        /// <summary>
        ///     The this.
        /// </summary>
        /// <param name="i">
        ///     The i.
        /// </param>
        /// <returns>
        ///     The <see cref="Node" />.
        /// </returns>
        public List<int> this[int i]
        {
            get => this.neighbourList[i];

            set => this.neighbourList[i] = value;
        }
    }
}