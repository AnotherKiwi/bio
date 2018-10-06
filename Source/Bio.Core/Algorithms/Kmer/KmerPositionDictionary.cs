using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bio.Algorithms.Kmer
{
    /// <summary>
    /// Wrapper for dictionary that maps kmer strings 
    /// to list of positions of occurrence. 
    /// </summary>
    public class KmerPositionDictionary
    {
        /// <summary>
        /// Maps kmer to list of positions of occurrence. 
        /// </summary>
        private readonly Dictionary<ISequence, IList<long>> kmerDictionary = new Dictionary<ISequence, IList<long>>(new SequenceEqualityComparer());

        /// <summary>
        /// Gets the number of elements present in this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return kmerDictionary.Count;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public IList<long> this[ISequence key]
        {
            get { return kmerDictionary[key]; }
            set { kmerDictionary[key] = value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the 
        /// kmer and corresponding list of positions.
        /// </summary>
        /// <returns>Enumerator over kmers.</returns>
        public Dictionary<ISequence, IList<long>>.Enumerator GetEnumerator()
        {
            return kmerDictionary.GetEnumerator();
        }

        /// <summary>
        /// Determines whether kmer dictionary contains specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>Boolean indicating if key exists.</returns>
        public bool ContainsKey(ISequence key)
        {
            return kmerDictionary.ContainsKey(key);
        }
    }
}
