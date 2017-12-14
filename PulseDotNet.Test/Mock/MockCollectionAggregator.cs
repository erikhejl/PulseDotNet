using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PulseDotNet.Collection;

namespace PulseDotNet.Test.Mock
{
    /// <summary>
    /// Mock concretion of abstract CollectionAggregator
    /// </summary>
    public class MockCollectionAggregator : CollectionAggregator<int, string>
    {        
        // 1 second interval, two frame history depth
        public MockCollectionAggregator(int historyDepth, AutoResetEvent autoReset) : base(1, historyDepth)
        {
            this.AggregationExecuted += (sender, args) => autoReset.Set();
        }

        protected override string AggregateTransform(IEnumerable<int> transformElements)
        {
            return transformElements.Sum().ToString();
        }
    }
}