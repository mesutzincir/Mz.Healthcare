using System.Text.Json;

namespace Mz.Healthcare.Api.Middlewares
{
    public class GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                
                context.Response.StatusCode = ex switch
                {
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = env.IsDevelopment()
                        ? ex.Message
                        : "Internal Server Error"
                };

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }

}
