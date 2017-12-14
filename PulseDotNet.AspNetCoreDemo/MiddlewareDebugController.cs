using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace PulseDotNet.AspNetCoreDemo
{
    [Route("demo")]
    public class MiddlewareDebugController : Controller
    {
        // GET api/values
        [HttpGet("requests")]
        public string TotalRequests()
        {
            return $"{RequestCounterMiddleware.RequestCount}";
        }

        [HttpGet("delay")]
        public void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        [HttpGet("error")]
        public void ServerError()
        {
            throw new DemoException();
        }
    }
}