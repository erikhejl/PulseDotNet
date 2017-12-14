namespace PulseDotNet
{
    /// <summary>
    /// APM results for a single request.
    /// </summary>
    public class RequestMetric
    {
        /// <summary>
        /// URL path for the API endpoint.
        /// </summary>
        public string EndpointPath;

        /// <summary>
        /// Http verb for the API endpoint.
        /// </summary>
        public string EndpointHttpVerb;
        
        /// <summary>
        /// Request latency/duration in milliseconds.
        /// </summary>
        public long Latency;

        /// <summary>
        /// Time spent idle during the request.
        /// </summary>
        public int IdleTime;
        
        /// <summary>
        /// Indicates that the request was terminated with a server error.
        /// </summary>
        /// <remarks>
        /// 0 - No Error, 1 - Error
        /// </remarks>
        public int Errors;

        /// <summary>
        /// Number of open simultaneous, concurrent requests in flight at the time of sampling.
        /// </summary>
        public int ConcurrentRequests;
    }
}