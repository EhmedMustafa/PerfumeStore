using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PerfumeStore.API.Middleware
{
    // Bütün handle edilməyən exception-ları tutur və standart JSON cavab qaytarır:
    //   { "message": "...", "code": "..." }
    // Dev mühitində stack trace də əlavə edilir.
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
                await WriteError(context, ex);
            }
        }

        private async Task WriteError(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted) return;

            var (status, code, message) = ex switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "unauthorized", "İcazə yoxdur"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "not_found", ex.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, "bad_request", ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, "bad_argument", ex.Message),
                _ => (HttpStatusCode.InternalServerError, "server_error", "Daxili xəta baş verdi")
            };

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            var payload = new
            {
                message = message,
                code = code,
                detail = _env.IsDevelopment() ? (ex.InnerException?.Message ?? ex.Message) : null,
                stack = _env.IsDevelopment() ? ex.StackTrace : null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }));
        }
    }
}
