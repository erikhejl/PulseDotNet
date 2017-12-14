using System.Collections.Generic;
using System.Linq;
using PulseDotNet.Collection;

namespace PulseDotNet
{
    /// <summary>
    /// Aggregate/transform request metrics. 
    /// </summary>
    public interface IRequestMetricTransformer : IEnumerableTransform<RequestMetric, RequestMetricAggregate>
    {}
    
    /// <summary>
    /// Aggregate/transform request metrics. 
    /// </summary>
    public class RequestMetricTransformer : IRequestMetricTransformer 
    {
        public IEnumerable<RequestMetricAggregate> Transform(IEnumerable<RequestMetric> source)
        {
            return source.GroupBy(m => new {m.EndpointHttpVerb, m.EndpointPath}).AsParallel()
                .Select(grp => grp.Aggregate(new RequestMetricAggregate
                    {
                        EndpointHttpVerb = grp.Key.EndpointHttpVerb,
                        EndpointPath = grp.Key.EndpointPath,
                        Requests = grp.Count()
                    },
                    (agg, next) => agg.Add(next)
                ));
        }
    }
}