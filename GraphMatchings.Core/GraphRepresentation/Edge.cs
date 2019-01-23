namespace GraphMatchings.Core.GraphRepresentation
{

    public class Edge
    {
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