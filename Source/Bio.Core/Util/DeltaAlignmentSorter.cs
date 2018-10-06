using System;
using System.Collections.Generic;

namespace Bio.Util
{
    /// <summary>
    /// DeltaAlignment sorter.
    /// Uses AVL tree to sort the deltaAligmments on FirstSequenceStart.
    /// </summary>
    public class DeltaAlignmentSorter
    {
        /// <summary>
        /// Intervals at which Hooks are needed.
        /// This value is decided on,
        ///     1. No of elements can be stored in "holderHooks".
        ///     2. Time required to travel in between Hooks.
        /// The value 1000 is decided as 1GB is the max known chromosome size and thus
        /// ~1Million entries will be added to the "holderHooks".
        /// </summary>
        private const int HooksIntervals = 1000;

        /// <summary>
        /// Holds minimum no of holders to be added when value of node being 
        /// added is more than the holders capacity.
        /// Adding holders one by one take more time.
        /// Thus allocate an appropriate number of holders at a time.
        /// </summary>
        private const int HoldersCapacityIncrementsBy = 1000000;

        /// <summary>
        /// Holds the end holder.
        /// </summary>
        private Holder endHolder;

        /// <summary>
        /// Capacity of this holder.
        /// </summary>
        private long holderCapacity;

        /// <summary>
        /// In worst scenario i.e, for 1GB chromosome ~1million entries of 
        /// holder's reference will be added. 
        /// For 100MB sequence ~105,000 entries will be added.
        /// Thus it is safe to use List for hooks.
        /// Note:  that first entry of hooks will start at HooksIntervals and not from zero.
        ///       as Root it self is a hook at zero position.
        /// </summary>
        private List<Holder> holderHooks = new List<Holder>();

        /// <summary>
        /// Initializes a new instance of the DeltaAlignmentSorter class.
        /// </summary>
        public DeltaAlignmentSorter()
        {
            holderHooks.Add(new Holder());
            endHolder = Root;
            holderCapacity = 1;
            IncreaseCapacityTo(HoldersCapacityIncrementsBy);
        }

        /// <summary>
        /// Initializes a new instance of the DeltaAlignmentSorter class with specified capacity.
        /// </summary>
        /// <param name="referenceSequenceLength">Reference sequence length.</param>
        public DeltaAlignmentSorter(long referenceSequenceLength)
        {
            holderHooks.Add(new Holder());
            endHolder = Root;
            holderCapacity = 1;
            IncreaseCapacityTo(referenceSequenceLength);
        }

        /// <summary>
        /// Gets the number of delta alignments present.
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// Gets the first holder.
        /// </summary>
        private Holder Root
        {
            get
            {
                return holderHooks[0];
            }
        }

        /// <summary>
        /// Adds id and value to sort.
        /// </summary>
        /// <param name="id">Id of the delta alignment.</param>
        /// <param name="value">Value to sort.</param>
        public void Add(long id, long value)
        {
            var node = new Node();
            node.ID = id;
            node.Value = value;
            Add(node);
        }

        /// <summary>
        /// Gets the ids which are sorted on value.
        /// </summary>
        /// <returns>IEnumerable of ids.</returns>
        public IEnumerable<long> GetSortedIds()
        {
            var holder = Root;
            while (holder != null)
            {
                var node = holder.ValueNode;
                while (node != null)
                {
                    yield return node.ID;
                    node = node.Next;
                }

                holder = holder.Right;
            }
        }

        /// <summary>
        /// Gets id and value pair sorted on value.
        /// </summary>
        /// <returns>IEnumerable of id and value pair.</returns>
        public IEnumerable<Node> GetSortedNodes()
        {
            var holder = Root;
            while (holder != null)
            {
                var node = holder.ValueNode;
                while (node != null)
                {
                    yield return node;
                    node = node.Next;
                }

                holder = holder.Right;
            }
        }

        /// <summary>
        /// Adds a node.
        /// </summary>
        /// <param name="node">Node to add.</param>
        public void Add(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var holder = GetHolderFor(node.Value);

            // add the node to the holder.
            if (holder.ValueNode == null)
            {
                holder.ValueNode = node;
            }
            else
            {
                var previousNode = holder.ValueNode;
                while (previousNode.Next != null)
                {
                    previousNode = previousNode.Next;
                }

                previousNode.Next = node;
            }

            Count++;
        }

        /// <summary>
        /// Increases the capacity to specified value.
        /// </summary>
        /// <param name="capacity">New capacity.</param>
        /// <returns>Returns the last holder.</returns>
        private void IncreaseCapacityTo(long capacity)
        {
            if (capacity <= holderCapacity)
            {
                return;
            }

            if (capacity < HoldersCapacityIncrementsBy)
            {
                capacity = HoldersCapacityIncrementsBy;
            }

            var index = holderCapacity - 1;
            var holder = endHolder;
            var remainder = (int)(index % HooksIntervals);
            while (index < capacity - 1)
            {
                var loopCount = HooksIntervals - remainder;
                for (var i = 0; i < loopCount && index < capacity - 1; i++)
                {
                    holder.Right = new Holder();
                    holder = holder.Right;
                    index++;
                }

                remainder = (int)(index % HooksIntervals);
                if (remainder == 0)
                {
                    holderHooks.Add(holder);
                }
            }

            endHolder = holder;
            holderCapacity = capacity;
        }

        /// <summary>
        /// Gets the holder for the specified value.
        /// </summary>
        /// <param name="value">Value for which holder is required.</param>
        private Holder GetHolderFor(long value)
        {
            var holder = Root;
            long index = 0;

            // node.value is start from zero thus capacity should be value + 1
            if (holderCapacity < value + 1)
            {
                var nextValue = HoldersCapacityIncrementsBy + holderCapacity;
                if (nextValue < value + 1)
                {
                    nextValue = value + 1;
                }

                // If holders are not present create holders.
                IncreaseCapacityTo(nextValue);
            }

            if (value > HooksIntervals)
            {
                var hooksindex = (int)(value / HooksIntervals);
                index = hooksindex * HooksIntervals;

                // index start from zero.
                holder = holderHooks[hooksindex];
            }

            // Navigate to the holder equal to the specified value.
            while (index < value)
            {
                holder = holder.Right;
                index++;
            }

            return holder;
        }
    }

    /// <summary>
    /// Class to hold nodes.
    /// Used to hold nodes having same value.
    /// </summary>
    internal class Holder
    {
        /// <summary>
        /// Gets or sets next holder.
        /// </summary>
        public Holder Right { get; set; }

        /// <summary>
        /// Gets or sets a node.
        /// </summary>
        public Node ValueNode { get; set; }
    }

    /// <summary>
    /// Class to hold id and value pair.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets or sets Id of the delta alignment.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets Value to sort.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// Gets or sets next node having same value.
        /// </summary>
        public Node Next { get; set; }
    }
}
