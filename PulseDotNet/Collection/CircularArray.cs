using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PulseDotNet.Collection
{
    /// <summary>
    /// Fixed sized buffer that evicts objects on a First In/First Out basis. 
    /// </summary>
    /// <typeparam name="T">Object type to be stored</typeparam>
    public class CircularArray<T> : ICollection<T>
    {
        /// <summary>
        /// Revolving index offset to serve as a relative "zero" index.
        /// </summary>
        internal int Offset;
        
        /// <summary>
        /// Backing store for the circular array.
        /// </summary>
        internal T[] Elements;

        public int Count => Elements.Length;
        public bool IsReadOnly => false;
        
        /// <summary>
        /// Construct a circular array from an existing array.
        /// </summary>
        /// <param name="init">array to initialize circular array with</param>
        public CircularArray(T[] init)
        {
            Elements = init;
        }

        /// <summary>
        /// Construct an empty circular array of a given size.
        /// </summary>
        /// <param name="size">Fixed size of the collection</param>
        public CircularArray(int size)
        {
            Elements = new T[size];
        }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="idx">zero based index</param>
        public T this[int idx]
        {
            get => Elements[AdjustedIndex(idx)];
            set => Elements[AdjustedIndex(idx)] = value;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new CircularArrayEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            lock (this)
            {
                Elements[Offset] = item;
                if (++Offset == Elements.Length) Offset = 0;
            }
        }

        public void Clear()
        {
            Elements = new T[Elements.Length];
        }

        public bool Contains(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return Elements.Contains(item);
        }

        public void CopyTo(T[] array, int copyIndex)
        {
            if(array == null) throw new ArgumentNullException(nameof(array));
            if(copyIndex < 0) throw new ArgumentOutOfRangeException(nameof(copyIndex), $"{nameof(copyIndex)} cannot be less than zero.");
            if(array.Length - copyIndex < Elements.Length) throw new ArgumentException(nameof(array), "Destination array was not long enough.");
            // optimize for simple scenarios
            if (Offset == 0)
            {
                Elements.CopyTo(array, copyIndex);
            }
            else
            {
                // wrapped copy
                for (int idx = Offset; idx < Elements.Length; idx++)
                {
                    array[copyIndex++] = Elements[idx];
                }
                for (int idx = 0; idx < Offset; idx++)
                {
                    array[copyIndex++] = Elements[idx];
                }
            }
        }

        public bool Remove(T item)
        {
            for (int rawIdx = 0; rawIdx < Elements.Length; rawIdx++)
            {
                if (Elements[rawIdx].Equals(item))
                {
                    int offsetIdx = OffesetIndex(rawIdx);
                    RemoveAt(offsetIdx);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove element at a specified index.
        /// </summary>
        /// <param name="idx">zero based index of element to be removed</param>
        public void RemoveAt(int idx)
        {
            ValidateIndex(idx);
            int upperBounds = Elements.Length - 1;
            for (; idx < upperBounds; idx++)
            {
                this[idx] = this[idx + 1];
            }
            this[upperBounds] = default(T);
        }
        
        /// <summary>
        /// Wrapping, offset adjusted index.
        /// </summary>
        /// <param name="idx">Offset relative index</param>
        /// <returns>Adjusted raw index</returns>
        private int AdjustedIndex(int idx)
        {
            ValidateIndex(idx);
            return (idx + Offset) % Elements.Length;
        }

        /// <summary>
        /// Opposite of AdjustedIndex.  Takes absolute position from the backing store 
        /// and determines the corresponding external collection index.
        /// </summary>
        private int OffesetIndex(int rawIdx)
        {
            int offsetIdx = rawIdx - Offset;
            offsetIdx = offsetIdx < 0
                ? Elements.Length + offsetIdx
                : offsetIdx;
            return offsetIdx;
        }

        void ValidateIndex(int idx)
        {
            if(idx < 0 || idx >= Elements.Length) throw new IndexOutOfRangeException();            
        }
    }    
}