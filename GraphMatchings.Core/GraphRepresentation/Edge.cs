namespace MatchingsCore.GraphRepresentation
{
    /// <summary>
    /// The edge.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        public Edge(int from, int to)
        {
            this.From = from;
            this.To = to;
        }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        public int To { get; set; }
    }
}
