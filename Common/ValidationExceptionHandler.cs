using FluentValidation;
using PortfolioApi.Common;

namespace PortfolioApi.Infrastructure;

public class ValidationExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteError(context, StatusCodes.Status400BadRequest, "Validation failed", ex.Errors.Select(e => e.ErrorMessage).ToList());
        }
        catch (ArgumentException ex)
        {
            await WriteError(context, StatusCodes.Status400BadRequest, "Validation failed", new List<string> { ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            await WriteError(context, StatusCodes.Status400BadRequest, "Validation failed", new List<string> { ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            await WriteError(context, StatusCodes.Status404NotFound, ex.Message);
        }
    }

    private static async Task WriteError(
        HttpContext context, int statusCode, string message, List<string>? errors = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var response = ApiResponse<object>.Fail(message, errors);
        await context.Response.WriteAsJsonAsync(response);
    }
}