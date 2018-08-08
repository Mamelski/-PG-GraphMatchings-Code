namespace MatchingsCore.GraphRepresentation
{
    /// <summary>
    /// The node.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether visited.
        /// </summary>
        public bool Visited { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public int Color { get; set; }
    }
}