using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace PulseDotNet.Collection
{
    /// <summary>
    /// Accumulate information and distill it into aggregated values.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TAggregate"></typeparam>
    public interface ICollectionAggregator<in TElement, TAggregate> 
    {        
        /// <summary>
        /// Rolling history of information that has been collected.
        /// </summary>
        /// <returns></returns>
        ICollection<TAggregate> GetHistory();
        
        /// <summary>
        /// Accumulate a piece of information.
        /// </summary>
        /// <param name="element"></param>
        void Add(TElement element);
    }

    public abstract class CollectionAggregator<T> : CollectionAggregator<T, T>
    {
        protected CollectionAggregator(int resolutionMilliseconds, int historyDepth) : base(resolutionMilliseconds, historyDepth)
        {}
    }

    public abstract class CollectionAggregator<TElement, TAggregate> : IDisposable, ICollectionAggregator<TElement, TAggregate>
    {
        /// <summary>
        /// Event raised when new aggregated values are generated.
        /// </summary>
        public event EventHandler<AggregationEventArgs<TAggregate>> AggregationExecuted; 
        
        protected int ResolutionMilliseconds { get; private set; }

        private ICollection<TElement> _currentElements;
        private readonly CircularArray<TAggregate> _aggregateHistory;
        private readonly Timer _collectionTimer;
        
        protected CollectionAggregator(int resolutionSeconds, int historyDepth)
        {
            _currentElements = new List<TElement>();
            _aggregateHistory = new CircularArray<TAggregate>(historyDepth);
            ResolutionMilliseconds = resolutionSeconds;
            _collectionTimer = new Timer(resolutionSeconds * 1000);
            _collectionTimer.Elapsed += CollectionTimerElapsed; 
            _collectionTimer.Start();
        }

        public void Add(TElement element)
        {
            _currentElements.Add(element);
        }
               
        public ICollection<TAggregate> GetHistory()
        {
            return _aggregateHistory.Elements;
        }
        
        /// <summary>
        /// Transform accumulated items into an item aggregation.
        /// </summary>
        /// <param name="transformElements">Collection of new items to be transformed</param>
        /// <returns>Aggregate value of elements</returns>
        protected abstract TAggregate AggregateTransform(IEnumerable<TElement> transformElements);
        
        void CollectionTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ICollection<TElement> oldElements = _currentElements;
            _currentElements = new List<TElement>();
            TAggregate aggregatedValue = AggregateTransform(oldElements);
            _aggregateHistory.Add(aggregatedValue);
            AggregationExecuted?.Invoke(this, new AggregationEventArgs<TAggregate>(aggregatedValue));
        }
        
        public void Dispose()
        {
            _collectionTimer?.Dispose();
        }
    }
}