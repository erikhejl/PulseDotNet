using System;
using System.Collections.Generic;
using System.Linq;
using PulseDotNet.Collection;

namespace PulseDotNet
{
    internal class HealthMetricsAggregator : CollectionAggregator<RequestMetric, IList<RequestMetricAggregate>>
    {
        public static HealthMetricsAggregator Instance { get; }

        static HealthMetricsAggregator()
        {
            Instance = new HealthMetricsAggregatorFactory().Create();
        }
        
        private readonly IRequestMetricTransformer _aggregateTransformer;
        
        public HealthMetricsAggregator(int resolutionMilliseconds, int historyDepth, IRequestMetricTransformer aggregateTransformer)
            : base(resolutionMilliseconds, historyDepth)
        {
            _aggregateTransformer = aggregateTransformer ?? throw new ArgumentNullException(nameof(aggregateTransformer));
        }

        protected override IList<RequestMetricAggregate> AggregateTransform(IEnumerable<RequestMetric> transformElements)
        {
            return _aggregateTransformer.Transform(transformElements).ToList();
        }        
    }
}