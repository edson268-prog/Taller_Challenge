using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Taller_Challenge_Backend.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred",
                Detail = _env.IsDevelopment() ? exception.Message : "Please contact the administrator",
                Instance = context.Request.Path
            };

            switch (exception)
            {
                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid request";
                    problemDetails.Detail = argEx.Message;
                    break;

                case KeyNotFoundException keyEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Resource not found";
                    problemDetails.Detail = keyEx.Message;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = "You are not authorized to perform this action";
                    break;

                case InvalidOperationException opEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Title = "Operation conflict";
                    problemDetails.Detail = opEx.Message;
                    break;

                case TimeoutException:
                    context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                    problemDetails.Status = StatusCodes.Status504GatewayTimeout;
                    problemDetails.Title = "Request timeout";
                    problemDetails.Detail = "The request took too long to process";
                    break;
            }

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
