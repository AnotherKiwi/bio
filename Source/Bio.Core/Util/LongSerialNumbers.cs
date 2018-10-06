﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Bio.Util
{
    /// <summary>
    /// Provides serial numbers to each Elements.
    /// </summary>
    /// <typeparam name="T">Type of elements to store.</typeparam>
    public class LongSerialNumbers<T>
    {
        #region Member variables
        private BigList<T> unSortedItems;
        private AATree<T, long> sortedItems;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes an instance of LongSerialNumbers class.
        /// </summary>
        public LongSerialNumbers() : this(Comparer<T>.Default) { }

        /// <summary>
        /// Initializes an instance of LongSerialNumbers class with specified comparer.
        /// </summary>
        /// <param name="comparer">Comparer to use for comparing two items.</param>
        public LongSerialNumbers(IComparer<T> comparer)
        {
            unSortedItems = new BigList<T>();
            sortedItems = new AATree<T, long>(comparer);
            sortedItems.DefaultValue = -1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of elements present in the LongSerialNumbers.
        /// </summary>
        public long Count
        {
            get
            {
                return unSortedItems.Count;
            }
        }
        #endregion

        /// <summary>
        /// Returns the serial number of an item. If the item has already been assigned a serial number, returns that number; 
        /// otherwise, assigns a new number to the item and returns that new number.
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The item's serial number</returns>
        public long GetNewOrOld(T item)
        {
            var serialNumber = sortedItems[item];

            if (serialNumber == -1)
            {
                serialNumber = unSortedItems.Count;
                unSortedItems.Add(item);
                sortedItems.Add(item, serialNumber);
            }

            return serialNumber;
        }

        /// <summary>
        /// Assigns a serial number to a new item. Raises an exception of the item is not new.
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The items serial number</returns>
        public long GetNew(T item)
        {
            var newSerialNumber = unSortedItems.Count;
            var isAdded = sortedItems.Add(item, newSerialNumber);
            if (!isAdded)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Properties.Resource.ExpectedItemToNotExist, item.ToString()));
            }
            else
            {
                unSortedItems.Add(item);
            }

            return newSerialNumber;
        }

        /// <summary>
        /// Finds the serial number of item to which a serial number has already been assigned. Raises an exception of the item is new.
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The serial number of that item.</returns>
        public long GetOld(T item)
        {
            long serialNumber = -1;
            var result = TryGetOld(item, out serialNumber);

            if (!result)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Properties.Resource.ExpectedItemToExist, item.ToString()));
            }

            return serialNumber;
        }

        /// <summary>
        /// The last serial number assigned.
        /// </summary>
        public long Last
        {
            get
            {
                return unSortedItems.Count - 1;
            }
        }

        /// <summary>
        /// Finds the serial number of item to which a serial number has already been assigned.
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="serialNumber">The serial number that was assigned to that item.</param>
        /// <returns>true if the item has previously been assigned a serial number; otherwise, false.</returns>
        public bool TryGetOld(T item, out long serialNumber)
        {
            var result = true;
            serialNumber = sortedItems[item];
            if (serialNumber == -1)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Tells if an item already has a serial number
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>true if the item has previously been assigned a serial number; otherwise, false.</returns>
        public bool Contains(T item)
        {
            long serialNumber=-1;
            var result = TryGetOld(item, out serialNumber);
            return result;
        }

    }
}
