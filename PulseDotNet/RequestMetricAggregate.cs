using System;
using Microsoft.AspNetCore.Http;

namespace PulseDotNet
{
    /// <summary>
    /// Summary of APM metrics over a time span for a specific API endpoint.
    /// </summary>
    public class RequestMetricAggregate : RequestMetric
    {
        /// <summary>
        /// Total number of requests received.
        /// </summary>
        public int Requests;

        public int MaxLatency;

        public int AvgLatency;
        
        
    }
}