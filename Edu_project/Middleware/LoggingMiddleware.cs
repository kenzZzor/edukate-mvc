using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EdukateMvc.Namespace.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
            => (_next, _logger) = (next, logger);

        public async Task Invoke(HttpContext ctx)
        {
            var sw = Stopwatch.StartNew();
            await _next(ctx);
            sw.Stop();
            _logger.LogInformation("{Method} {Path} => {StatusCode} ({Elapsed} ms)",
                ctx.Request.Method,
                ctx.Request.Path,
                ctx.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
    }
}
