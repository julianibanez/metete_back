using Metete.Api.Infraestructure.Exceptions;
using System.Text.Json;

namespace Metete.Api.Infraestructure.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            _logger.LogError(exception.Message);

            string message = exception.Message;

            switch (exception)  
            {
                case BusinessRuleException:
                    response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    break;
                case DBOperationFailedException:
                case BadHttpRequestException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case EntityNotFoundException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case LoginFailedException:
                case PasswordRecoveryException:
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    message = "Internal Server Error. Please contact your system administrator.";

                    break;
            }

            string result = JsonSerializer.Serialize(new { Message = message });
            await response.WriteAsync(result);
        }
    }
}