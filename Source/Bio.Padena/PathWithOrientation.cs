using System;
using System.Collections.Generic;
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.Padena
{
    /// <summary>
    /// Structure that stores list of nodes in path, 
    /// along with path orientation.
    /// </summary>
    public class PathWithOrientation
    {
        /// <summary>
        /// List of nodes in path.
        /// </summary>
        private List<DeBruijnNode> nodes;

        /// <summary>
        /// Initializes a new instance of the PathWithOrientation class.
        /// </summary>
        /// <param name="node1">First node to add.</param>
        /// <param name="node2">Second node to add.</param>
        /// <param name="orientation">Path orientation.</param>
        public PathWithOrientation(DeBruijnNode node1, DeBruijnNode node2, bool orientation)
        {
            if (node1 == null)
            {
                throw new ArgumentNullException(nameof(node1));
            }

            if (node2 == null)
            {
                throw new ArgumentNullException(nameof(node2));
            }

            nodes = new List<DeBruijnNode> { node1, node2 };
            IsSameOrientation = orientation;
        }

        /// <summary>
        /// Initializes a new instance of the PathWithOrientation class.
        /// Copies the input path info to a new one.
        /// </summary>
        /// <param name="other">Path info to copy.</param>
        public PathWithOrientation(PathWithOrientation other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            nodes = new List<DeBruijnNode>(other.Nodes);
            IsSameOrientation = other.IsSameOrientation;
        }

        /// <summary>
        /// Gets the list of nodes in path.
        /// </summary>
        public IList<DeBruijnNode> Nodes
        {
            get { return nodes; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether path orientation is same or opposite
        /// with respect to the start node of the path.
        /// </summary>
        public bool IsSameOrientation { get; set; }
    }
}
