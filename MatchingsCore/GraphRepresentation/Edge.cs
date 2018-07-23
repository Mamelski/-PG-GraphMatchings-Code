namespace MatchingsCore.GraphRepresentation
{
    /// <summary>
    /// The edge.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Gets or sets the weigth.
        /// </summary>
        public int Weigth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        public Node Destination { get; set; }
    }
}
