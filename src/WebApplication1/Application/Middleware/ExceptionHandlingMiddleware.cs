using Serilog;
using System.Net;
using System.Text.Json;

namespace WebApplication1.Application.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error(exception, "An unhandled exception occurred.");

            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest, // Erro de entrada inválida
                InvalidOperationException => HttpStatusCode.BadRequest, // Erro de regra de negócio
                KeyNotFoundException => HttpStatusCode.NotFound, // Recurso não encontrado
                _ => HttpStatusCode.InternalServerError // Erros desconhecidos
            };

            var response = new
            {
                type = statusCode switch
                {
                    HttpStatusCode.BadRequest => "ValidationError",
                    HttpStatusCode.NotFound => "ResourceNotFound",
                    _ => "InternalServerError"
                },
                error = exception.Message,
                detail = exception.InnerException?.Message ?? "An unexpected error occurred."
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
