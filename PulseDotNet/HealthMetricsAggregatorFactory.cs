namespace PulseDotNet
{
    internal class HealthMetricsAggregatorFactory
    {
        public  HealthMetricsAggregator Create()
        {
            int resolutionSeconds = 10;
            int historyDepth = 6;
            return new HealthMetricsAggregator(resolutionSeconds, historyDepth, new RequestMetricTransformer());
        }
    }
}