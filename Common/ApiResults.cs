namespace PortfolioApi.Common;

public static class ApiResults
{
    public static IResult Ok<T>(T data, string message = "Operation successful")
        => Results.Ok(ApiResponse<T>.Ok(data, message));

    public static IResult Ok(string message = "Operation successful")
        => Results.Ok(ApiResponse<object>.Ok(new { }, message));

    public static IResult Created<T>(string uri, T data, string message = "Resource created")
        => Results.Created(uri, ApiResponse<T>.Ok(data, message));

    public static IResult Fail(string message, List<string>? errors = null)
        => Results.BadRequest(ApiResponse<object>.Fail(message, errors));

    public static IResult NotFound(string message = "Resource not found")
        => Results.NotFound(ApiResponse<object>.NotFound(message));
}