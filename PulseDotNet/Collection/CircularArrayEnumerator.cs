using System;
using System.Collections;
using System.Collections.Generic;

namespace PulseDotNet.Collection
{
    /// <summary>
    /// Enumerate through a circular array as if it were a traditional fixed size array.
    /// </summary>
    /// <typeparam name="T">Array element type</typeparam>
    public class CircularArrayEnumerator<T> : IEnumerator<T>
    {
        private int _localIdx;
        private readonly CircularArray<T> _source;
        
        public CircularArrayEnumerator(CircularArray<T> sourceArray)
        {
            _source = sourceArray ?? throw new ArgumentNullException(nameof(sourceArray));
            Reset();
        }

        public T Current => _source[_localIdx];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _localIdx++;
            if (_localIdx == _source.Elements.Length) _localIdx = 0;
            // not sure if it's really "bad" that we allow enumerator wrapping, since 
            // this *is* a circular array after all.
            return _localIdx == _source.Offset;
        }

        public void Reset()
        {
            _localIdx = _source.Offset;
        }
        
        public void Dispose()
        {
        }
    }
}