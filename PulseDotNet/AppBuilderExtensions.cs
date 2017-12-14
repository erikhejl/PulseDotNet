using Microsoft.AspNetCore.Builder;

namespace PulseDotNet
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Add Pulse.NET profiling to the request pipeline.
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHealthMonitoring(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<HealthMonitoringMiddleware>();
        }
    }
}