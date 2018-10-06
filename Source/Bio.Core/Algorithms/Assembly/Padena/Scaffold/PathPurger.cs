using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;

namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Removes containing paths and merge Overlapping scaffold paths.
    /// Containing Paths
    /// -------------- Contig 1
    ///     --------   Contig 2
    /// Overlapping Paths
    /// --------        Contig 1 
    ///     ---------   Contig 2.
    /// </summary>
    public class PathPurger : IPathPurger
    {
        /// <summary>
        /// Input list of scaffold paths.
        /// </summary>
        private IList<ScaffoldPath> internalScaffoldPaths;

        /// <summary>
        /// Removes containing paths and merge overlapping paths.
        /// </summary>
        /// <param name="scaffoldPaths">Input paths/scaffold.</param>
        public void PurgePath(IList<ScaffoldPath> scaffoldPaths)
        {
            if (scaffoldPaths != null && 0 != scaffoldPaths.Count)
            {
                internalScaffoldPaths = scaffoldPaths.AsParallel().OrderBy(t => t.Count).ToList();
                var isUpdated = true;
                var isConsumed = new bool[internalScaffoldPaths.Count];

                while (isUpdated)
                {
                    isUpdated = false;
                    for (var index = 0; index < internalScaffoldPaths.Count; index++)
                    {
                        if (null != internalScaffoldPaths[index] &&
                            0 != internalScaffoldPaths[index].Count && !isConsumed[index])
                        {
                            isUpdated |=
                                SearchContainingAndOverlappingPaths(internalScaffoldPaths[index], isConsumed);
                        }
                        else
                        {
                            isConsumed[index] = true;
                        }
                    }
                }

                UpdatePath(isConsumed);
                scaffoldPaths.Clear();
                ((List<ScaffoldPath>)scaffoldPaths).AddRange(internalScaffoldPaths);
            }
        }

        /// <summary>
        /// Remove containing paths.
        /// </summary>
        /// <param name="scaffoldPath">Current path.</param>
        /// <param name="path">Path to be compared with.</param>
        /// <returns>Containing paths or not.</returns>
        private static bool RemoveContainingPaths(
            ScaffoldPath scaffoldPath,
            ScaffoldPath path)
        {
            if (scaffoldPath.Count >= path.Count)
            {
                if (path.All(t => scaffoldPath.Where(k => k.Key == t.Key).ToList().Count > 0))
                {
                    return true;
                }
                return false;
            }
            
            if (scaffoldPath.All(t => path.Where(k => k.Key == t.Key).ToList().Count > 0))
            {
                scaffoldPath.Clear();
                scaffoldPath.AddRange(path);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Removes Overlapping paths by generating pairwise overlaps between paths.
        /// </summary>
        /// <param name="scaffoldPath">Current path.</param>
        /// <param name="path">Path to be compared with.</param>
        /// <returns>Overlapping paths or not.</returns>
        private static bool RemoveOverlappingPaths(
            ScaffoldPath scaffoldPath,
            ScaffoldPath path)
        {
            // Generate Overlap Matrix [Similar To Pairwise Overlap aligner] 
            var matrix = new bool[scaffoldPath.Count, path.Count];
            for (var index = 0; index < scaffoldPath.Count; index++)
            {
                for (var index1 = 0; index1 < path.Count; index1++)
                {
                    matrix.SetValue(scaffoldPath[index].Key == path[index1].Key, index, index1);
                }
            }

            // Search in last row for a match.
            var startPosOfRow = -1;
            for (var index = scaffoldPath.Count - 1; index >= 0; index--)
            {
                if ((bool)matrix.GetValue(index, path.Count - 1))
                {
                    var index1 = 1;
                    while (path.Count - 1 - index1 >= 0 && index - index1 >= 0)
                    {
                        if ((bool)matrix.GetValue(index - index1, path.Count - 1 - index1))
                        {
                            index1++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (path.Count - 1 - index1 <= 0 || index - index1 <= 0)
                    {
                        startPosOfRow = index;
                        break;
                    }
                }
            }

            // Search in last column for match.
            var startPosOfCol = -1;
            for (var index = path.Count - 2; index >= 0; index--)
            {
                if ((bool)matrix.GetValue(scaffoldPath.Count - 1, index))
                {
                    var index1 = 1;
                    while (scaffoldPath.Count - 1 - index1 > 0 && index - index1 > 0)
                    {
                        if ((bool)matrix.GetValue(scaffoldPath.Count - 1 - index1, index - index1))
                        {
                            index1++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (scaffoldPath.Count - 1 - index1 <= 0 || index - index1 <= 0)
                    {
                        startPosOfCol = index;
                        break;
                    }
                }
            }

            if (startPosOfCol != -1 || startPosOfRow != -1)
            {
                if (startPosOfRow >= startPosOfCol)
                {
                    StitchPath(scaffoldPath, path, startPosOfRow, path.Count - 1);
                    return true;
                }
                StitchPath(scaffoldPath, path, scaffoldPath.Count - 1, startPosOfCol);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Stitches overlapping paths and update the path.
        /// </summary>
        /// <param name="scaffoldPath">Current path.</param>
        /// <param name="path">Path to be compared with.</param>
        /// <param name="pos">End position of overlap in first path.</param>
        /// <param name="pos1">End position of overlap in second path.</param>
        private static void StitchPath(
            IList<KeyValuePair<Node, Edge>> scaffoldPath,
            IList<KeyValuePair<Node, Edge>> path,
            int pos,
            int pos1)
        {
            if (pos == scaffoldPath.Count - 1)
            {
                for (var index = pos1 + 1; index < path.Count; index++)
                {
                    scaffoldPath.Add(path[index]);
                }
            }
            else
            {
                for (var index = pos + 1; index < scaffoldPath.Count; index++)
                {
                    path.Add(scaffoldPath[index]);
                }

                scaffoldPath.Clear();
                ((List<KeyValuePair<Node, Edge>>)scaffoldPath).AddRange(path);
            }
        }

        /// <summary>
        /// Search for containing and overlapping paths.
        /// </summary>
        /// <param name="scaffoldPath">Current Path.</param>
        /// <param name="isConsumed">Path status.</param>
        /// <returns>Update list or not.</returns>
        private bool SearchContainingAndOverlappingPaths(
            ScaffoldPath scaffoldPath,
            bool[] isConsumed)
        {
            var isUpdated = false;
            for (var index = 0; index < internalScaffoldPaths.Count; index++)
            {
                if (!isConsumed[index] && scaffoldPath != internalScaffoldPaths[index])
                {
                    if (RemoveContainingPaths(scaffoldPath, internalScaffoldPaths[index]))
                    {
                        isConsumed[index] = true;
                        isUpdated = true;
                    }
                    else
                    {
                        if (RemoveOverlappingPaths(scaffoldPath, internalScaffoldPaths[index]))
                        {
                            isConsumed[index] = true;
                            isUpdated = true;
                        }
                    }
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// Removes all consumed path.
        /// </summary>
        /// <param name="isConsumed">Status of paths.</param>
        private void UpdatePath(bool[] isConsumed)
        {
            IList<ScaffoldPath> scaffoldPaths = new List<ScaffoldPath>();
            for (var index = 0; index < internalScaffoldPaths.Count; index++)
            {
                if (!(bool)isConsumed.GetValue(index))
                {
                    scaffoldPaths.Add(internalScaffoldPaths[index]);
                }
            }

            internalScaffoldPaths.Clear();
            ((List<ScaffoldPath>)internalScaffoldPaths).AddRange(scaffoldPaths);
        }
    }
}
