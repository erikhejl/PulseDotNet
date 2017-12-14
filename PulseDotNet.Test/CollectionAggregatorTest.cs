using System.Linq;
using System.Threading;
using PulseDotNet.Test.Mock;
using Xunit;

namespace PulseDotNet.Test
{
    public class CollectionAggregatorTest
    {
        [Fact]
        public void TwoFrames()
        {
            var frameResetEvent = new AutoResetEvent(false);
            var aggregator = new MockCollectionAggregator(3, frameResetEvent);            
            
            aggregator.Add(1);
            aggregator.Add(2);
            aggregator.Add(3);
            frameResetEvent.WaitOne();          
            
            aggregator.Add(4);
            aggregator.Add(5);
            frameResetEvent.WaitOne();
            
            // assertions
            var history = aggregator.GetHistory().ToList();
            Assert.Equal("6", history[0]);
            Assert.Equal("9", history[1]);
            Assert.Null(history[2]);
        }
    }
}