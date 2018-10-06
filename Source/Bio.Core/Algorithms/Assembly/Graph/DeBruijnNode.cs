using System;
using System.Collections.Generic;
using Bio.Algorithms.Kmer;
using System.Diagnostics;

namespace Bio.Algorithms.Assembly.Graph
{
    /// <summary>
    /// Represents a node in the De Bruijn graph
    /// A node is associated with a k-mer. 
    /// Also holds adjacency information with other nodes.
    /// </summary>
    [DebuggerDisplay("Seq = {" + nameof(ORIGINAL_SYMBOLS) + "}")]
    public class DeBruijnNode
    {
        //temp addition
        /// <summary>
        /// 
        /// </summary>
        public string ORIGINAL_SYMBOLS
        {
            get {
                return new Sequence(DnaAlphabet.Instance, GetOriginalSymbols(6)).ConvertToString(0,6);
            }
        }

        #region Node Operations Masks
        private const byte NodeOperationMaskLeftExtension       = 56;   // 8,16,24 
        private const byte NodeOperationMaskRightExtension      = 7;    // 1,2,4
        private const byte NodeOperationMaskIsMarkedForDelete   = 64;
        private const byte NodeOperationMaskIsValidExtension    = 128;
        #endregion

        #region Node Orientation Masks
        private const byte NodeMaskRightExtension0 = 1;
        private const byte NodeMaskRightExtension1 = 2;
        private const byte NodeMaskRightExtension2 = 4;
        private const byte NodeMaskRightExtension3 = 8;

        private const byte NodeMaskLeftExtension0 = 16;
        private const byte NodeMaskLeftExtension1 = 32;
        private const byte NodeMaskLeftExtension2 = 64;
        private const byte NodeMaskLeftExtension3 = 128;
        #endregion

        #region Node Info Masks
        private const byte NodeInfoMaskDeleted = 1;
        private const byte NodeInfoVisitedFlag = 2;
        #endregion

        /// <summary>
        /// Holds a flag to indicate whether this node is deleted or not.
        /// </summary>
        private byte _nodeInfo;

        /// <summary>
        /// Holds the value of validextension required, is node marked for deletion , right extension count and left extension count
        /// in 8, 7, 4 to 6 and 1 to 3 bits respectively.
        /// </summary>
        private byte _nodeOperations;

        /// <summary>
        /// Stores the node orientation.
        /// First 4 bits Forward links orientation, next 4 bits reverse links orientation (from Right to Left).
        /// If bit values are 1 then same orientation. If bit values are 0 then orientation is different.
        /// </summary>
        private byte _nodeOrientation;

        /// <summary>
        /// Stores the valid Node extensions
        /// First 4 bits Forward links orientation, next 4 bits reverse links orientation (from Right to Left).
        /// If bit values are 0 then valid extension. If bit values are 1 then not a valid extension.
        /// </summary>
        private byte _validNodeExtensions;

        /// <summary>
        /// Initializes a new instance of the DeBruijnNode class.
        /// </summary>
        public DeBruijnNode(KmerData32 value, byte count)
        {
            NodeValue = value;
            KmerCount = count;
        }

        /// <summary>
        /// Gets or sets the value of an DeBrujinNode.
        /// </summary>
        public KmerData32 NodeValue { get; set; }

        /// <summary>
        /// Gets or sets the number of duplicate kmers in the DeBrujin graph.
        /// </summary>
        public byte KmerCount { get; set; }

        /// <summary>
        /// Gets or sets the Left node, used by binary tree.
        /// </summary>
        public DeBruijnNode Left { get; set; }

        /// <summary>
        /// Gets or sets the Right Node, used by binary tree.
        /// </summary>
        public DeBruijnNode Right { get; set; }

        /// <summary>
        /// Gets a value indicating whether the node is marked for deletion or not.
        /// </summary>
        public bool IsMarkedForDelete
        {
            get
            {
                return ((_nodeOperations & NodeOperationMaskIsMarkedForDelete) == NodeOperationMaskIsMarkedForDelete);
            }

            private set
            {
                if (value)
                {
                    _nodeOperations = (byte)(_nodeOperations | NodeOperationMaskIsMarkedForDelete);
                }
                else
                {
                    _nodeOperations = (byte)(_nodeOperations & (~NodeOperationMaskIsMarkedForDelete));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node is deleted or not.
        /// Note: As we are only periodically not deleting any nodes from the Tree, this flag helps to
        /// identify which nodes are deleted. 
        /// 
        /// TODO: Ensure this variable cannot be modified without modifying the parent graphs node count
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                return (_nodeInfo & NodeInfoMaskDeleted) == NodeInfoMaskDeleted;
            }

            set
            {
                if (value)
                {
                    _nodeInfo = (byte)(_nodeInfo | NodeInfoMaskDeleted);
                }
                else
                {
                    throw new ArgumentException("Deleted nodes should not be restored, use IsMarkedForDelete if unsure of the nodes future.");
                }
            }
        }

        /// <summary>
        /// A flag that can be used to determine if the node has been visited 
        /// during a specific step
        /// </summary>
        public bool IsVisited
        {
            get
            {
                return (_nodeInfo & NodeInfoVisitedFlag) == NodeInfoVisitedFlag;
            }

            set
            {
                if (value)
                {
                    _nodeInfo = (byte)(_nodeInfo | NodeInfoVisitedFlag);
                }
                else
                {
                    _nodeInfo = (byte)(_nodeInfo & ~NodeInfoVisitedFlag);
                }
            }
        }
      

        /// <summary>
        /// Gets the number of extension nodes.
        /// </summary>
        public int ExtensionsCount
        {
            get
            {
                return LeftExtensionNodesCount + RightExtensionNodesCount;
            }
        }

        /// <summary>
        /// Gets or sets the number of right extension nodes.
        /// </summary>
        public byte RightExtensionNodesCount
        {
            get
            {
                var count = (_nodeOperations & NodeOperationMaskRightExtension);
                if (ValidExtensionsRequried)
                {
                    if (InvalidRightExtension0) 
                    { 
                        count--; 
                    }

                    if (InvalidRightExtension1) 
                    { 
                        count--; 
                    }

                    if (InvalidRightExtension2) 
                    { 
                        count--; 
                    }

                    if (InvalidRightExtension3) 
                    { 
                        count--; 
                    }
                }

                return (byte)count;
            }

            set
            {
                if (value > 4)
                {
                    throw new ArgumentException("Value cant be more than 4");
                }

                _nodeOperations = (byte)(_nodeOperations & ~(NodeOperationMaskRightExtension));
                _nodeOperations = (byte)(_nodeOperations | (NodeOperationMaskRightExtension & value));
            }
        }

        /// <summary>
        /// Gets or sets the number of left extension nodes.
        /// </summary>
        public byte LeftExtensionNodesCount
        {
            get
            {
                var count = ((_nodeOperations & NodeOperationMaskLeftExtension) >> 3);
                if (ValidExtensionsRequried)
                {
                    if (InvalidLeftExtension0) 
                    { 
                        count--; 
                    }
                    
                    if (InvalidLeftExtension1) 
                    { 
                        count--; 
                    }
                    
                    if (InvalidLeftExtension2) 
                    { 
                        count--; 
                    }
                    
                    if (InvalidLeftExtension3) 
                    { 
                        count--; 
                    }
                }

                return (byte)count;
            }
            set
            {
                if (value > 4)
                {
                    throw new ArgumentException("Value cant be more than 4");
                }

                _nodeOperations = (byte)(_nodeOperations & (~NodeOperationMaskLeftExtension));
                _nodeOperations = (byte)(_nodeOperations | (NodeOperationMaskLeftExtension & (value << 3)));
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets or sets a value indicating whether node has valid extension or not.
        /// </summary>
        private bool ValidExtensionsRequried
        {
            get
            {
                return ((_nodeOperations & NodeOperationMaskIsValidExtension) == NodeOperationMaskIsValidExtension);
            }
            set
            {
                if (value)
                {
                    _nodeOperations = (byte)(_nodeOperations | NodeOperationMaskIsValidExtension);
                }
                else
                {
                    _nodeOperations = (byte)(_nodeOperations & (~NodeOperationMaskIsValidExtension));
                }
            }
        }

        #region Node Orientation Private Properties

        private bool OrientationRightExtension0
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskRightExtension0) == NodeMaskRightExtension0); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskRightExtension0);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskRightExtension0));
                }
            }
        }

        private bool OrientationRightExtension1
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskRightExtension1) == NodeMaskRightExtension1); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskRightExtension1);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskRightExtension1));
                }
            }
        }

        private bool OrientationRightExtension2
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskRightExtension2) == NodeMaskRightExtension2); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskRightExtension2);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskRightExtension2));
                }
            }
        }

        private bool OrientationRightExtension3
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskRightExtension3) == NodeMaskRightExtension3); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskRightExtension3);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskRightExtension3));
                }
            }
        }

        private bool OrientationLeftExtension0
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskLeftExtension0) == NodeMaskLeftExtension0); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskLeftExtension0);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskLeftExtension0));
                }
            }
        }

        private bool OrientationLeftExtension1
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskLeftExtension1) == NodeMaskLeftExtension1); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskLeftExtension1);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskLeftExtension1));
                }
            }
        }

        private bool OrientationLeftExtension2
        {
            get 
            { 
                return ((_nodeOrientation & NodeMaskLeftExtension2) == NodeMaskLeftExtension2); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskLeftExtension2);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskLeftExtension2));
                }
            }
        }

        private bool OrientationLeftExtension3
        {
            get
            { 
                return ((_nodeOrientation & NodeMaskLeftExtension3) == NodeMaskLeftExtension3); 
            }

            set
            {
                if (value)
                {
                    _nodeOrientation = (byte)(_nodeOrientation | NodeMaskLeftExtension3);
                }
                else
                {
                    _nodeOrientation = (byte)(_nodeOrientation & (~NodeMaskLeftExtension3));
                }
            }
        }
        #endregion

        #region Node Extensions Private Properties

        private bool InvalidRightExtension0
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskRightExtension0) == NodeMaskRightExtension0);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskRightExtension0);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskRightExtension0));
                }
            }
        }

        private bool InvalidRightExtension1
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskRightExtension1) == NodeMaskRightExtension1);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskRightExtension1);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskRightExtension1));
                }
            }
        }

        private bool InvalidRightExtension2
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskRightExtension2) == NodeMaskRightExtension2);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskRightExtension2);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskRightExtension2));
                }
            }
        }

        private bool InvalidRightExtension3
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskRightExtension3) == NodeMaskRightExtension3);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskRightExtension3);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskRightExtension3));
                }
            }
        }

        private bool InvalidLeftExtension0
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskLeftExtension0) == NodeMaskLeftExtension0);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskLeftExtension0);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskLeftExtension0));
                }
            }
        }

        private bool InvalidLeftExtension1
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskLeftExtension1) == NodeMaskLeftExtension1);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskLeftExtension1);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskLeftExtension1));
                }
            }
        }

        private bool InvalidLeftExtension2
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskLeftExtension2) == NodeMaskLeftExtension2);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskLeftExtension2);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskLeftExtension2));
                }
            }
        }

        private bool InvalidLeftExtension3
        {
            get
            {
                return ((_validNodeExtensions & NodeMaskLeftExtension3) == NodeMaskLeftExtension3);
            }

            set
            {
                if (value)
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions | NodeMaskLeftExtension3);
                }
                else
                {
                    _validNodeExtensions = (byte)(_validNodeExtensions & (~NodeMaskLeftExtension3));
                }
            }
        }
        #endregion

        #region Node Extension Properties

        /// <summary>
        /// Gets or sets the RightExtension node for dna symbol 'A'.
        /// </summary>
        private DeBruijnNode RightExtension0 { get; set; }

        /// <summary>
        /// Gets or sets the RightExtension node for dna symbol 'C'.
        /// </summary>
        private DeBruijnNode RightExtension1 { get; set; }

        /// <summary>
        /// Gets or sets the RightExtension node for dna symbol 'G'.
        /// </summary>
        private DeBruijnNode RightExtension2 { get; set; }

        /// <summary>
        /// Gets or sets the RightExtension node for dna symbol 'T'.
        /// </summary>
        private DeBruijnNode RightExtension3 { get; set; }

        /// <summary>
        /// Gets or sets the Left Extension node for dna symbol 'A'.
        /// </summary>
        private DeBruijnNode LeftExtension0 { get; set; }

        /// <summary>
        /// Gets or sets the Left Extension node for dna symbol 'C'.
        /// </summary>
        private DeBruijnNode LeftExtension1 { get; set; }

        /// <summary>
        /// Gets or sets the Left Extension node for dna symbol 'G'.
        /// </summary>
        private DeBruijnNode LeftExtension2 { get; set; }

        /// <summary>
        /// Gets or sets the Left Extension node for dna symbol 'T'.
        /// </summary>
        private DeBruijnNode LeftExtension3 { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Marks the LeftExtensions of the current node as invalid.
        /// </summary>
        /// <param name="node">Debruijn node which matches one of the left extensions of the current node.</param>
        public bool MarkLeftExtensionAsInvalid(DeBruijnNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (LeftExtension0 == node)
            {
                InvalidLeftExtension0 = true;
                return true;
            }
            else if (LeftExtension1 == node)
            {
                InvalidLeftExtension1 = true;
                return true;
            }
            else if (LeftExtension2 == node)
            {
                InvalidLeftExtension2 = true;
                return true;
            }
            else if (LeftExtension3 == node)
            {
                InvalidLeftExtension3 = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Marks the RightExtensions of the current node as invalid.
        /// </summary>
        /// <param name="node">Debruijn node which matches one of the right extensions of the current node.</param>
        public bool MarkRightExtensionAsInvalid(DeBruijnNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (RightExtension0 == node)
            {
                InvalidRightExtension0 = true;
                return true;
            }
            else if (RightExtension1 == node)
            {
                InvalidRightExtension1 = true;
                return true;
            }
            else if (RightExtension2 == node)
            {
                InvalidRightExtension2 = true;
                return true;
            }
            else if (RightExtension3 == node)
            {
                InvalidRightExtension3 = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the extension nodes those are marked for deletion.
        /// </summary>
        public void RemoveMarkedExtensions()
        {
            // If node is marked for deletion, ignore it. No need for any update.
            if (IsMarkedForDelete)
            {
                return;
            }

            if (RightExtension0 != null && RightExtension0.IsMarkedForDelete)
            {
                RightExtension0 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension1 != null && RightExtension1.IsMarkedForDelete)
            {
                RightExtension1 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension2 != null && RightExtension2.IsMarkedForDelete)
            {
                RightExtension2 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension3 != null && RightExtension3.IsMarkedForDelete)
            {
                RightExtension3 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (LeftExtension0 != null && LeftExtension0.IsMarkedForDelete)
            {
                LeftExtension0 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension1 != null && LeftExtension1.IsMarkedForDelete)
            {
                LeftExtension1 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension2 != null && LeftExtension2.IsMarkedForDelete)
            {
                LeftExtension2 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension3 != null && LeftExtension3.IsMarkedForDelete)
            {
                LeftExtension3 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }
        }
                
        /// <summary>
        /// Sets the extension nodes of the current node.
        /// </summary>
        /// <param name="isForwardDirection">True indicates Right extension and false indicates left extension.</param>
        /// <param name="sameOrientation">Orientation of the connecting edge.</param>
        /// <param name="extensionNode">Node to which the extension is to be set.</param>
        public void SetExtensionNode(bool isForwardDirection, bool sameOrientation, DeBruijnNode extensionNode)
        {
            if (extensionNode == null)
            {
                return;
            }

            lock (this)
            {
                // First 4 bits Forward links orientation, next 4 bits reverse links orientation
                // If bit values are 1 then same orientation. If bit values are 0 then orientation is different.
                if (isForwardDirection)
                {                   
                  
                        if (RightExtension0 == null)
                        {
                            RightExtension0 = extensionNode;
                            OrientationRightExtension0 = sameOrientation;
                        }
                        else if (RightExtension1 == null)
                        {
                            RightExtension1 = extensionNode;
                            OrientationRightExtension1 = sameOrientation;
                        }
                        else if (RightExtension2 == null)
                        {
                            RightExtension2 = extensionNode;
                            OrientationRightExtension2 = sameOrientation;
                        }
                        else if (RightExtension3 == null)
                        {
                            RightExtension3 = extensionNode;
                            OrientationRightExtension3 = sameOrientation;
                        }
                        else
                        {
                            throw new ArgumentException("Can't set more than four extensions.");
                        }
                  
                    RightExtensionNodesCount += 1;
                }
                else
                {
                   
                        if (LeftExtension0 == null)
                        {
                            LeftExtension0 = extensionNode;
                            OrientationLeftExtension0 = sameOrientation;
                        }
                        else if (LeftExtension1 == null)
                        {
                            LeftExtension1 = extensionNode;
                            OrientationLeftExtension1 = sameOrientation;
                        }
                        else if (LeftExtension2 == null)
                        {
                            LeftExtension2 = extensionNode;
                            OrientationLeftExtension2 = sameOrientation;
                        }
                        else if (LeftExtension3 == null)
                        {
                            LeftExtension3 = extensionNode;
                            OrientationLeftExtension3 = sameOrientation;
                        }
                        else
                        {
                            throw new ArgumentException("Can't set more than four extensions.");
                        }
                    
                    LeftExtensionNodesCount += 1;
                }
            }
        }

        /// <summary>
        /// Returns all the left extension and right extension nodes of the current node.
        /// </summary>
        /// <returns>Left extension and right extension nodes.</returns>
        public IEnumerable<DeBruijnNode> GetExtensionNodes()
        {
            if (LeftExtension0 != null)
            {
                yield return LeftExtension0;
            }

            if (LeftExtension1 != null)
            {
                yield return LeftExtension1;
            }

            if (LeftExtension2 != null)
            {
                yield return LeftExtension2;
            }

            if (LeftExtension3 != null)
            {
                yield return LeftExtension3;
            }

            if (RightExtension0 != null)
            {
                yield return RightExtension0;
            }

            if (RightExtension1 != null)
            {
                yield return RightExtension1;
            }

            if (RightExtension2 != null)
            {
                yield return RightExtension2;
            }

            if (RightExtension3 != null)
            {
                yield return RightExtension3;
            }
        }

        /// <summary>
        /// Retrives the list of right extension nodes along with the orientation.
        /// </summary>
        /// <returns>Dictionary with the right extension node and the orientation.</returns>
        public Dictionary<DeBruijnNode, bool> GetRightExtensionNodesWithOrientation()
        {
            lock (this)
            {
                var extensions = new Dictionary<DeBruijnNode, bool>();

                if (ValidExtensionsRequried)
                {
                    if (RightExtension0 != null && !InvalidRightExtension0)
                    {
                        if (!extensions.ContainsKey(RightExtension0))
                        {
                            extensions.Add(RightExtension0, OrientationRightExtension0);
                        }
                    }

                    if (RightExtension1 != null && !InvalidRightExtension1)
                    {
                        if (!extensions.ContainsKey(RightExtension1))
                        {
                            extensions.Add(RightExtension1, OrientationRightExtension1);
                        }
                    }

                    if (RightExtension2 != null && !InvalidRightExtension2)
                    {
                        if (!extensions.ContainsKey(RightExtension2))
                        {
                            extensions.Add(RightExtension2, OrientationRightExtension2);
                        }
                    }

                    if (RightExtension3 != null && !InvalidRightExtension3)
                    {
                        if (!extensions.ContainsKey(RightExtension3))
                        {
                            extensions.Add(RightExtension3, OrientationRightExtension3);
                        }
                    }
                }
                else
                {
                    if (RightExtension0 != null)
                    {
                        if (!extensions.ContainsKey(RightExtension0))
                        {
                            extensions.Add(RightExtension0, OrientationRightExtension0);
                        }
                    }

                    if (RightExtension1 != null)
                    {
                        if (!extensions.ContainsKey(RightExtension1))
                        {
                            extensions.Add(RightExtension1, OrientationRightExtension1);
                        }
                    }

                    if (RightExtension2 != null)
                    {
                        if (!extensions.ContainsKey(RightExtension2))
                        {
                            extensions.Add(RightExtension2, OrientationRightExtension2);
                        }
                    }

                    if (RightExtension3 != null)
                    {
                        if (!extensions.ContainsKey(RightExtension3))
                        {
                            extensions.Add(RightExtension3, OrientationRightExtension3);
                        }
                    }
                }

                return extensions;
            }
        }

        /// <summary>
        /// Retrives the list of left extension nodes along with the orientation.
        /// </summary>
        /// <returns>Dictionary with the left extension node and the orientation.</returns>
        public Dictionary<DeBruijnNode, bool> GetLeftExtensionNodesWithOrientation()
        {
            lock (this)
            {
                var extentions = new Dictionary<DeBruijnNode, bool>();

                if (ValidExtensionsRequried)
                {
                    if (LeftExtension0 != null && !InvalidLeftExtension0)
                    {
                        if (!extentions.ContainsKey(LeftExtension0))
                        {
                            extentions.Add(LeftExtension0, OrientationLeftExtension0);
                        }
                    }

                    if (LeftExtension1 != null && !InvalidLeftExtension1)
                    {
                        if (!extentions.ContainsKey(LeftExtension1))
                        {
                            extentions.Add(LeftExtension1, OrientationLeftExtension1);
                        }
                    }

                    if (LeftExtension2 != null && !InvalidLeftExtension2)
                    {
                        if (!extentions.ContainsKey(LeftExtension2))
                        {
                            extentions.Add(LeftExtension2, OrientationLeftExtension2);
                        }
                    }

                    if (LeftExtension3 != null && !InvalidLeftExtension3)
                    {
                        if (!extentions.ContainsKey(LeftExtension3))
                        {
                            extentions.Add(LeftExtension3, OrientationLeftExtension3);
                        }
                    }
                }
                else
                {
                    if (LeftExtension0 != null)
                    {
                        if (!extentions.ContainsKey(LeftExtension0))
                        {
                            extentions.Add(LeftExtension0, OrientationLeftExtension0);
                        }
                    }

                    if (LeftExtension1 != null)
                    {
                        if (!extentions.ContainsKey(LeftExtension1))
                        {
                            extentions.Add(LeftExtension1, OrientationLeftExtension1);
                        }
                    }

                    if (LeftExtension2 != null)
                    {
                        if (!extentions.ContainsKey(LeftExtension2))
                        {
                            extentions.Add(LeftExtension2, OrientationLeftExtension2);
                        }
                    }

                    if (LeftExtension3 != null)
                    {
                        if (!extentions.ContainsKey(LeftExtension3))
                        {
                            extentions.Add(LeftExtension3, OrientationLeftExtension3);
                        }
                    }
                }

                return extentions;
            }
        }

        /// <summary>
        /// Removes edge corresponding to the node from appropriate data structure,
        /// after checking whether given node is part of left or right extensions.
        /// Thread-safe method.
        /// </summary>
        /// <param name="node">Node for which extension is to be removed.</param>
        public void RemoveExtensionThreadSafe(DeBruijnNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            lock (this)
            {
                if (RightExtension0 == node)
                {
                    RightExtension0 = null;
                    RightExtensionNodesCount--;
                    return;
                }

                if (RightExtension1 == node)
                {
                    RightExtension1 = null;
                    RightExtensionNodesCount--;
                    return;
                }

                if (RightExtension2 == node)
                {
                    RightExtension2 = null;
                    RightExtensionNodesCount--;
                    return;
                }

                if (RightExtension3 == node)
                {
                    RightExtension3 = null;
                    RightExtensionNodesCount--;
                    return;
                }
            }

            lock (this)
            {
                if (LeftExtension0 == node)
                {
                    LeftExtension0 = null;
                    LeftExtensionNodesCount--;
                    return;
                }

                if (LeftExtension1 == node)
                {
                    LeftExtension1 = null;
                    LeftExtensionNodesCount--;
                    return;
                }

                if (LeftExtension2 == node)
                {
                    LeftExtension2 = null;
                    LeftExtensionNodesCount--;
                    return;
                }

                if (LeftExtension3 == node)
                {
                    LeftExtension3 = null;
                    LeftExtensionNodesCount--;
                    return;
                }
            }
        }

      
        /// <summary>
        /// Retrieves all the Left extension nodes of the current node.
        /// </summary>
        /// <returns>Right extension nodes.</returns>
        public IEnumerable<DeBruijnNode> GetLeftExtensionNodes()
        {
            if (ValidExtensionsRequried)
            {
                if (LeftExtension0 != null && !InvalidLeftExtension0)
                {
                    yield return LeftExtension0;
                }

                if (LeftExtension1 != null && !InvalidLeftExtension1)
                {
                    yield return LeftExtension1;
                }

                if (LeftExtension2 != null && !InvalidLeftExtension2)
                {
                    yield return LeftExtension2;
                }

                if (LeftExtension3 != null && !InvalidLeftExtension3)
                {
                    yield return LeftExtension3;
                }
            }
            else
            {
                if (LeftExtension0 != null)
                {
                    yield return LeftExtension0;
                }

                if (LeftExtension1 != null)
                {
                    yield return LeftExtension1;
                }

                if (LeftExtension2 != null)
                {
                    yield return LeftExtension2;
                }

                if (LeftExtension3 != null)
                {
                    yield return LeftExtension3;
                }
            }
        }

        /// <summary>
        /// Retrieves all the Right extension nodes of the current node.
        /// </summary>
        /// <returns>Right extension nodes.</returns>
        public IEnumerable<DeBruijnNode> GetRightExtensionNodes()
        {
            if (ValidExtensionsRequried)
            {
                if (RightExtension0 != null && !InvalidRightExtension0)
                {
                    yield return RightExtension0;
                }

                if (RightExtension1 != null && !InvalidRightExtension1)
                {
                    yield return RightExtension1;
                }

                if (RightExtension2 != null && !InvalidRightExtension2)
                {
                    yield return RightExtension2;
                }

                if (RightExtension3 != null && !InvalidRightExtension3)
                {
                    yield return RightExtension3;
                }
            }
            else
            {
                if (RightExtension0 != null)
                {
                    yield return RightExtension0;
                }

                if (RightExtension1 != null)
                {
                    yield return RightExtension1;
                }

                if (RightExtension2 != null)
                {
                    yield return RightExtension2;
                }

                if (RightExtension3 != null)
                {
                    yield return RightExtension3;
                }
            }
        }

        /// <summary>
        /// Sets whether valid extensions are required or not.
        /// </summary>
        public void ComputeValidExtensions()
        {
            ValidExtensionsRequried = true;
        }

        /// <summary>
        /// Deletes all the extension marked for deletion and sets the node extensions as valid.
        /// </summary>
        public void UndoAmbiguousExtensions()
        {
            // done with the valid extensions set vaidExtensionsRequired to false.
            ValidExtensionsRequried = false;

            RemoveMarkedExtensions();

            // mark all extensions as valid.
            _validNodeExtensions = 0;
        }

        /// <summary>
        /// Removes all the invalid extensions permanently.
        /// </summary>
        public void PurgeInvalidExtensions()
        {
            if (RightExtension0 != null && InvalidRightExtension0)
            {
                RightExtension0 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension1 != null && InvalidRightExtension1)
            {
                RightExtension1 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension2 != null && InvalidRightExtension2)
            {
                RightExtension2 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (RightExtension3 != null && InvalidRightExtension3)
            {
                RightExtension3 = null;
                lock (this)
                {
                    RightExtensionNodesCount--;
                }
            }

            if (LeftExtension0 != null && InvalidLeftExtension0)
            {
                LeftExtension0 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension1 != null && InvalidLeftExtension1)
            {
                LeftExtension1 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension2 != null && InvalidLeftExtension2)
            {
                LeftExtension2 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }

            if (LeftExtension3 != null && InvalidLeftExtension3)
            {
                LeftExtension3 = null;
                lock (this)
                {
                    LeftExtensionNodesCount--;
                }
            }
        }

        /// <summary>
        /// Marks the node for deletion.
        /// </summary>
        public void MarkNodeForDelete()
        {
            IsMarkedForDelete = true;
        }

        /// <summary>
        /// Makes extension edge corresponding to the node invalid,
        /// after checking whether given node is part of left or right extensions.
        /// Not Thread-safe. Use lock at caller if required.
        /// </summary>
        /// <param name="node">Node for which extension is to be made invalid.</param>
        public void MarkExtensionInvalid(DeBruijnNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!MarkRightExtensionAsInvalid(node))
            {
                MarkLeftExtensionAsInvalid(node);
            }
        }

        /// <summary>
        /// Gets the original symbols.
        /// </summary>
        /// <param name="kmerLength">Length of the kmer.</param>
        /// <returns>Return the decompressed kmer data.</returns>
        public byte[] GetOriginalSymbols(int kmerLength)
        {
            return NodeValue.GetOriginalSymbols(kmerLength);
        }

        /// <summary>
        /// Gets the reverse complement of original symbols.
        /// </summary>
        /// <param name="kmerLength">Length of the kmer.</param>
        /// <returns>Returns the reverse complement of the current node value.</returns>
        public byte[] GetReverseComplementOfOriginalSymbols(int kmerLength)
        {
            return NodeValue.GetReverseComplementOfOriginalSymbols(kmerLength);
        }

        /// <summary>
        /// Checks whether the node value (kmer data) is palindrome or not.
        /// </summary>
        /// <returns>True if the node value is palindrome otherwise false.</returns>
        public bool IsPalindrome(int kmerLength)
        {
            return NodeValue.IsPalindrome(kmerLength);
        }
    }
}
