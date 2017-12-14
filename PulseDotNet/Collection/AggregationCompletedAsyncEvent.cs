using System;
using System.ComponentModel;

namespace PulseDotNet.Collection
{
    public class AggregationEventArgs<T> : EventArgs
    {
        public T AggregatedValue { get; }

        public AggregationEventArgs(T value)
        {
            AggregatedValue = value;
        }
    }
}