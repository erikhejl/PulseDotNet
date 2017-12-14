using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PulseDotNet.Test
{
    public class RequestMetricTransformerTest
    {
        private IRequestMetricTransformer _transformer;

        public RequestMetricTransformerTest()
        {
            _transformer = new RequestMetricTransformer();           
        }
        
        [Fact]
        public void Transform_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => _transformer.Transform(null));
        }
        
        [Fact]
        public void Transform()
        {
            var metrics = CreateTestRequestMetrics();
            var aggregate = _transformer.Transform(metrics).ToList();
            Assert.Equal(3, aggregate.Count);
            // measure GET /Resource1
            var get1 = aggregate.First(a => EndpointKey(a) == "GET /Resource1");
            Assert.Equal(4, get1.Requests);
            Assert.Equal(2, get1.Errors);
            Assert.Equal(30, get1.IdleTime);
            Assert.Equal(235, get1.Latency);
            // measure POST /Resource1
            var post1 = aggregate.First(a => EndpointKey(a) == "POST /Resource1");
            Assert.Equal(2, post1.Requests);
            Assert.Equal(0, post1.Errors);
            Assert.Equal(35, post1.IdleTime);
            Assert.Equal(375, post1.Latency);
            // measure GET /Resource2
            var get2 = aggregate.First(a => EndpointKey(a) == "GET /Resource2");
            Assert.Equal(1, get2.Requests);
            Assert.Equal(0, get2.Errors);
            Assert.Equal(5, get2.IdleTime);
            Assert.Equal(20, get2.Latency);            
        }
        
        IEnumerable<RequestMetric> CreateTestRequestMetrics()
        {
            var metrics = new RequestMetric[]
            {
                new RequestMetric
                {
                    EndpointHttpVerb = "GET",
                    EndpointPath = "/Resource1",
                    Errors = 0,
                    IdleTime = 15,
                    Latency = 125
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "GET",
                    EndpointPath = "/Resource2",
                    Errors = 0,
                    IdleTime = 5,
                    Latency = 20
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "GET",
                    EndpointPath = "/Resource1",
                    Errors = 1,
                    IdleTime = 0,
                    Latency = 5
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "GET",
                    EndpointPath = "/Resource1",
                    Errors = 0,
                    IdleTime = 15,
                    Latency = 100
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "POST",
                    EndpointPath = "/Resource1",
                    Errors = 0,
                    IdleTime = 15,
                    Latency = 250
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "POST",
                    EndpointPath = "/Resource1",
                    Errors = 0,
                    IdleTime = 20,
                    Latency = 125
                },
                new RequestMetric
                {
                    EndpointHttpVerb = "GET",
                    EndpointPath = "/Resource1",
                    Errors = 1,
                    IdleTime = 0,
                    Latency = 5
                }
            };
            return metrics;
        }
        
        string EndpointKey(RequestMetricAggregate rma)
        {
            return $"{rma.EndpointHttpVerb} {rma.EndpointPath}";
        }
    }
}