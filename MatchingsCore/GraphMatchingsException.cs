namespace MatchingsCore
{
    using System;

    /// <summary>
    /// The graph matchings exception.
    /// </summary>
    public class GraphMatchingsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMatchingsException"/> class.
        /// </summary>
        public GraphMatchingsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMatchingsException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public GraphMatchingsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphMatchingsException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public GraphMatchingsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}