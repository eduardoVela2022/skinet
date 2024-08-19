// Imports
using System.Net;
using System.Text.Json;
using API.Errors;

// File path
namespace API.Middleware;

// The middleware to handle exceptions
public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    // Middle ware method to catch exceptions of other middleware
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continue to the next middleware
            await next(context);
        }
        catch (Exception ex)
        {
            // If an exception arrises use this method
            await HandleExceptionAsync(context, ex, env);
        }
    }

    // Handles any exception
    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception ex,
        IHostEnvironment env
    )
    {
        // Sets the content type and status code of the response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Creates the api response
        var response = env.IsDevelopment()
            ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiErrorResponse(
                context.Response.StatusCode,
                ex.Message,
                "Internal server error"
            );

        // Names the properties of the json file in camel case
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        // Creates the json object
        var json = JsonSerializer.Serialize(response, options);

        // Returns the json object
        return context.Response.WriteAsync(json);
    }
}
