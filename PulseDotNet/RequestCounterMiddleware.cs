using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PulseDotNet
{
    public class RequestCounterMiddleware
    {
        public static int RequestCount;
        private RequestDelegate _next;

        public RequestCounterMiddleware(RequestDelegate next)
        {
            Contract.Requires(next != null);
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Invoke(HttpContext httpContext)
        {
            Task.Run(() => RequestCount++);
            return _next.Invoke(httpContext);
        }
    }
}
