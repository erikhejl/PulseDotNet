using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PulseDotNet
{
    /// <summary>
    /// Aggregate app health metrics.
    /// </summary>
    /// <remarks>
    /// Implements legacy middleware convention and future middleware contract.
    /// </remarks>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HealthMonitoringMiddleware 
    {
        static readonly List<HttpContext> ConcurrentRequests = new List<HttpContext>();
        private readonly RequestDelegate _next;

        public HealthMonitoringMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
                
        public async Task InvokeAsync(HttpContext context)
        {
            // consider pooling stopwatches?
            var stopwatch = new Stopwatch();
            ConcurrentRequests.Add(context);
            var requestMetrics = new RequestMetric
            {
                EndpointHttpVerb = context.Request.Method,
                EndpointPath = context.Request.Path,
                ConcurrentRequests = ConcurrentRequests.Count
            };
            try
            {
                stopwatch.Start();
                await _next.Invoke(context);    
            }
            catch (Exception)
            {
                requestMetrics.Errors = 1;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                requestMetrics.Latency = stopwatch.ElapsedMilliseconds;
                HealthMetricsAggregator.Instance.Add(requestMetrics);
                ConcurrentRequests.Remove(context);
            }           
        }
    }
}