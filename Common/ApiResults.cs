using Microsoft.AspNetCore.Mvc;

namespace PortfolioApi.Common;

public static class ApiResults
{
    public static IActionResult Ok<T>(T data, string message = "Operation successful")
        => new OkObjectResult(ApiResponse<T>.Ok(data, message));

    public static IActionResult Ok(string message = "Operation successful")
        => new OkObjectResult(ApiResponse<object>.Ok(new { }, message));

    public static IActionResult Created<T>(string uri, T data, string message = "Resource created")
        => new CreatedResult(uri, ApiResponse<T>.Ok(data, message));

    public static IActionResult Fail(string message, List<string>? errors = null)
        => new BadRequestObjectResult(ApiResponse<object>.Fail(message, errors));

    public static IActionResult Unauthorized(string message = "Unauthorized")
        => new UnauthorizedObjectResult(ApiResponse<object>.Fail(message));

    public static IActionResult Forbidden(string message = "Forbidden")
        => new ObjectResult(ApiResponse<object>.Fail(message))
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

    public static IActionResult NotFound(string message = "Resource not found")
        => new NotFoundObjectResult(ApiResponse<object>.NotFound(message));
}