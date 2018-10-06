using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.Padena
{
    /// <summary>
    /// Redundant links are caused by single point mutations occuring in middle of reads.
    /// This class implements the methods for detecting redundant paths, 
    /// and then removing all but one path.
    /// </summary>
    public class RedundantPathsPurger : IGraphErrorPurger
    {
        #region Fields, Constructor, Properties
        /// <summary>
        /// Threshold for length of redundant paths.
        /// </summary>
        private int pathLengthThreshold;

        /// <summary>
        /// Holds reference to assembler graph.
        /// </summary>
        private DeBruijnGraph graph;

        /// <summary>
        /// Initializes a new instance of the RedundantPathsPurger class.
        /// Takes user parameter for threshold. 
        /// </summary>
        /// <param name="length">Threshold length.</param>
        public RedundantPathsPurger(int length)
        {
            pathLengthThreshold = length;
        }

        /// <summary>
        /// Gets the name of the algorithm.
        /// </summary>
        public string Name
        {
            get { return Properties.Resource.RedundantPathsPurger; }
        }

        /// <summary>
        /// Gets the description of the algorithm.
        /// </summary>
        public string Description
        {
            get { return Properties.Resource.RedundantPathsPurgerDescription; }
        }

        /// <summary>
        /// Gets or sets threshold for length of redundant paths.
        /// </summary>
        public int LengthThreshold
        {
            get { return pathLengthThreshold; }
            set { pathLengthThreshold = value; }
        }
        #endregion

        /// <summary>
        /// Detect nodes that are on redundant paths. 
        /// Start from any node that has ambiguous (more than one) extensions.
        /// From this node, trace path for each extension until either they 
        /// converge to a single node or threshold length is exceeded. 
        /// In case they converge, we have a set of redundant paths. 
        /// We pick the best path based on the kmer counts of the path nodes.
        /// All paths other than the best one are returned for removal.
        /// Locks: Method only does reads. No locking necessary here or its callees. 
        /// </summary>
        /// <param name="deBruijnGraph">De Bruijn Graph.</param>
        /// <returns>List of path nodes to be deleted.</returns>
        public DeBruijnPathList DetectErroneousNodes(DeBruijnGraph deBruijnGraph)
        {
            if (deBruijnGraph == null)
            {
                throw new ArgumentNullException(nameof(deBruijnGraph));
            }

            DeBruijnGraph.ValidateGraph(deBruijnGraph);
            graph = deBruijnGraph;

            var redundantPaths = new List<DeBruijnPathList>();
            Parallel.ForEach(
                deBruijnGraph.GetNodes(),
                node =>
                {
                    // Need to check for both left and right extensions for ambiguity.
                    if (node.RightExtensionNodesCount > 1)
                    {
                        TraceDivergingExtensionPaths(node, node.GetRightExtensionNodesWithOrientation(), true, redundantPaths);
                    }

                    if (node.LeftExtensionNodesCount > 1)
                    {
                        TraceDivergingExtensionPaths(node, node.GetLeftExtensionNodesWithOrientation(), false, redundantPaths);
                    }
                });

            redundantPaths = RemoveDuplicates(redundantPaths);
            return DetachBestPath(redundantPaths);
        }

        /// <summary>
        /// Removes nodes that are part of redundant paths. 
        /// </summary>
        /// <param name="deBruijnGraph">De Bruijn graph.</param>
        /// <param name="nodesList">Path nodes to be deleted.</param>
        public void RemoveErroneousNodes(DeBruijnGraph deBruijnGraph, DeBruijnPathList nodesList)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(deBruijnGraph));
            }

            DeBruijnGraph.ValidateGraph(deBruijnGraph);

            if (nodesList == null)
            {
                throw new ArgumentNullException(nameof(nodesList));
            }

            graph = deBruijnGraph;

            // Neighbors of all nodes have to be updated.
            var deleteNodes = new HashSet<DeBruijnNode>(
                nodesList.Paths.AsParallel().SelectMany(nl => nl.PathNodes));

            // Update extensions for deletion
            // No need for read-write lock as deleteNode's dictionary is being read, 
            // and only other graph node's dictionaries are updated.
            Parallel.ForEach(
                deleteNodes,
                node =>
                {
                    foreach (var extension in node.GetExtensionNodes())
                    {
                        // If the neighbor is also to be deleted, there is no use of updation in that case
                        if (!deleteNodes.Contains(extension))
                        {
                            extension.RemoveExtensionThreadSafe(node);
                        }
                    }
                });

            // Delete nodes from graph
            graph.RemoveNodes(deleteNodes);
        }

        /// <summary>
        /// Extract best path from the list of paths in each cluster.
        /// Take off the best path from list and return rest of the paths
        /// for removal.
        /// </summary>
        /// <param name="pathClusters">List of path clusters.</param>
        /// <returns>List of path nodes to be removed.</returns>
        private static DeBruijnPathList DetachBestPath(List<DeBruijnPathList> pathClusters)
        {
            return new DeBruijnPathList(
                pathClusters.AsParallel().SelectMany(paths => ExtractBestPath(paths).Paths));
        }

        /// <summary>
        /// Extract best path from list of paths. For the current cluster 
        /// of paths, return only those that should be removed.
        /// </summary>
        /// <param name="divergingPaths">List of redundant paths.</param>
        /// <returns>List of paths nodes to be deleted.</returns>
        private static DeBruijnPathList ExtractBestPath(DeBruijnPathList divergingPaths)
        {
            // Find "best" path. Except for best path, return rest for removal 
            var bestPathIndex = GetBestPath(divergingPaths);

            var bestPath = divergingPaths.Paths[bestPathIndex];
            divergingPaths.Paths.RemoveAt(bestPathIndex);

            // There can be overlap between redundant paths.
            // Remove path nodes that occur in best path
            foreach (var path in divergingPaths.Paths)
            {
                path.RemoveAll(n => bestPath.PathNodes.Contains(n));
            }

            return divergingPaths;
        }

        /// <summary>
        /// Gets the best path from the list of diverging paths.
        /// Path that has maximum sum of 'count' of belonging k-mers is best.
        /// In case there are multiple 'best' paths, we arbitrarily return one of them.
        /// </summary>
        /// <param name="divergingPaths">List of diverging paths.</param>
        /// <returns>Index of the best path.</returns>
        private static int GetBestPath(DeBruijnPathList divergingPaths)
        {
            // We find the index of the 'best' path.
            long max = -1;
            var maxIndex = -1;

            // Path that has the maximum sum of 'count' of belonging k-mers is the winner
            for (var i = 0; i < divergingPaths.Paths.Count; i++)
            {
                long sum = divergingPaths.Paths[i].PathNodes.Sum(n => n.KmerCount);
                if (sum > max)
                {
                    max = sum;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        /// <summary>
        /// Gets start node of redundant path cluster
        /// All paths in input are part of a redundant path cluster
        /// So all of them have the same start and the end node.
        /// Return the first node of first path.
        /// </summary>
        /// <param name="paths">List of redundant paths.</param>
        /// <returns>Start node of redundant path cluster.</returns>
        private static DeBruijnNode GetStartNode(DeBruijnPathList paths)
        {
            return paths.Paths.First().PathNodes.First();
        }

        /// <summary>
        /// Gets end node of redundant path cluster
        /// All paths in input are part of a redundant path cluster
        /// So all of them have the same start and the end node.
        /// Return the last node of first path.
        /// </summary>
        /// <param name="paths">List of redundant paths.</param>
        /// <returns>End node of redundant path cluster.</returns>
        private static DeBruijnNode GetEndNode(DeBruijnPathList paths)
        {
            return paths.Paths.First().PathNodes.Last();
        }

        /// <summary>
        /// Some set of paths will appear twice, one traced in forward direction
        /// and other in opposite. This method eliminate duplicates.
        /// </summary>
        /// <param name="redundantPathClusters">List of path cluster.</param>
        /// <returns>List of unique path clusters.</returns>
        private static List<DeBruijnPathList> RemoveDuplicates(List<DeBruijnPathList> redundantPathClusters)
        {
            // Divide the list into two groups. One with paths that do not 
            // have duplicates, and one with paths that do not have duplicate
            var uniqueAndDuplicatedPaths =
            redundantPathClusters.AsParallel().GroupBy(pc1 =>
                redundantPathClusters.Any(pc2 =>
                    GetStartNode(pc1) == GetEndNode(pc2) && GetEndNode(pc1) == GetStartNode(pc2))).ToList();

            var uniquePaths = new List<DeBruijnPathList>();
            foreach (var group in uniqueAndDuplicatedPaths)
            {
                if (!group.Key)
                {
                    // Add all paths that do have duplicates to final list
                    uniquePaths.AddRange(group);
                }
                else
                {
                    // Each element in this list contains a duplicate in the list
                    // Add only those where the start node has a sequence that is
                    // lexicographically greater than the end node sequence. This
                    // operation will eliminate duplicates effectively.
                    uniquePaths.AddRange(
                        group.AsParallel().Where(pc =>
                                GetStartNode(pc).NodeValue.CompareTo(
                                GetEndNode(pc).NodeValue) >= 0));
                }
            }

            return uniquePaths;
        }

        /// <summary>
        /// Traces diverging paths in given direction.
        /// For each path in the set of diverging paths, extend path by one node
        /// at a time. Continue this until all diverging paths converge to a 
        /// single node or length threshold is exceeded.
        /// If paths converge, add path cluster containing list of redundant 
        /// path nodes to list of redundant paths and return.
        /// </summary>
        /// <param name="startNode">Node at starting point of divergence.</param>
        /// <param name="divergingNodes">List of diverging nodes.</param>
        /// <param name="isForwardExtension">Bool indicating direction of divergence.</param>
        /// <param name="redundantPaths">List of redundant paths.</param>
        private void TraceDivergingExtensionPaths(
            DeBruijnNode startNode,
            Dictionary<DeBruijnNode, bool> divergingNodes,
            bool isForwardExtension,
            List<DeBruijnPathList> redundantPaths)
        {
            var divergingPaths = new List<PathWithOrientation>(
                divergingNodes.Select(n =>
                    new PathWithOrientation(startNode, n.Key, n.Value)));
            var divergingPathLengh = 2;

            // Extend paths till length threshold is exceeded.
            // In case paths coverge within threshold, we break out of while.
            while (divergingPathLengh <= pathLengthThreshold)
            {
                // Extension is possible only if end point of all paths has exactly one extension
                // In case extensions count is 0, no extensions possible for some path (or)
                // if extensions is more than 1, they are diverging further. Not considered a redundant path
                if (divergingPaths.Any(p => ((isForwardExtension ^ p.IsSameOrientation) ?
                      p.Nodes.Last().LeftExtensionNodesCount : p.Nodes.Last().RightExtensionNodesCount) != 1))
                {
                    return;
                }

                // Extend each path in cluster. While performing path extension 
                // also keep track of whether they have converged
                var hasConverged = true;
                foreach (var path in divergingPaths)
                {
                    var endNode = path.Nodes.Last();
                    var extensions
                        = (isForwardExtension ^ path.IsSameOrientation) ? endNode.GetLeftExtensionNodesWithOrientation() : endNode.GetRightExtensionNodesWithOrientation();

                    var nextNode = extensions.First();
                    if (path.Nodes.Contains(nextNode.Key))
                    {
                        // Loop in path
                        return;
                    }
                    else
                    {
                        // Update path orientation
                        path.IsSameOrientation = !(path.IsSameOrientation ^ nextNode.Value);
                        path.Nodes.Add(nextNode.Key);

                        // Check if paths so far are converged
                        if (hasConverged && nextNode.Key != divergingPaths.First().Nodes.Last())
                        {
                            // Last node added is different. Paths do not converge
                            hasConverged = false;
                        }
                    }
                }

                divergingPathLengh++;

                // Paths have been extended. Check for convergence
                if (hasConverged)
                {
                    // Note: all paths have the same end node.
                    lock (redundantPaths)
                    {
                        // Redundant paths found
                        redundantPaths.Add(new DeBruijnPathList(divergingPaths.Select(p => new DeBruijnPath(p.Nodes))));
                    }

                    return;
                }
            }
        }
    }
}
