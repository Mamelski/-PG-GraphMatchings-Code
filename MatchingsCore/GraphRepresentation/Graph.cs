namespace MatchingsCore.GraphRepresentation
{
    
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The graph.
    /// </summary>
    public abstract class Graph 
    {
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public abstract Node this[int index]
        {
            get;
            set;
        }

        /*
        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public abstract IEnumerator<Node> GetEnumerator();

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        */
    }
}
