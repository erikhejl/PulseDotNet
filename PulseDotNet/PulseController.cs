using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PulseDotNet
{
    /// <inheritdoc />
    /// <summary>
    /// PulseDotNet API to expose performance metrics in consuming applications.
    /// </summary>
    /// <remarks>
    /// Probably turn this into some sort of GraphQL service when it's all said and done.
    /// Individual metric entpoints mostly for testing and training.
    /// </remarks>
    [Route("pulse")]
    public class PulseController : ControllerBase
    {
        readonly HealthMetricsAggregator _metrics = HealthMetricsAggregator.Instance;
        
        /// <summary>
        /// Simple test to see if services are running.
        /// </summary>
        /// <returns>Static HTTP 200 with content of "echo".</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {            
            return Ok("echo");
        }

        /// <summary>
        /// Total request load.
        /// </summary>
        /// <returns></returns>
        [HttpGet("load")]
        public IActionResult Load()
        {            
            return new JsonResult(_metrics.GetHistory());
        }
        
        /// <summary>
        /// Operation response time metrics represent individual end user experience.
        /// </summary>
        /// <returns></returns>
        [HttpGet("reflex")]
        public IActionResult Reflex()
        {
            return NotImplemented();
        }

        /// <summary>
        /// Concurrent requests and overall load being serviced.
        /// </summary>
        /// <returns></returns>
        [HttpGet("flow")]
        public IActionResult Flow()
        {
            return NotImplemented();
        }
        
        /// <summary>
        /// Efficiency and utilization metrics to indicate where services are waiting on out of process I/O.
        /// </summary>
        /// <returns></returns>
        [HttpGet("utilization")]
        public IActionResult Utilization()
        {
            return NotImplemented();
        }

        // aka volatility
        /// <summary>
        /// Server error metrics to indicate faulted states. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("vitality")]
        public IActionResult Vitality()
        {
            return NotImplemented();
        }
        
        IActionResult NotImplemented()
        {
            return new ContentResult{StatusCode = 500, Content = "Not Implemented"};
        }
    }
}