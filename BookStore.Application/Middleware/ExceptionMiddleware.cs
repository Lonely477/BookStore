using System.Net;

namespace BookStore.Application.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        (int StatusCode, string Message) response = (
            context.Response.StatusCode,
            Message: $"Internal Server Error. An unexpected error occurred."
        );

        return context.Response.WriteAsJsonAsync(response);
    }
}
