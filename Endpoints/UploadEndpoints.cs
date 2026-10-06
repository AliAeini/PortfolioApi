using PortfolioApi.Services;

namespace PortfolioApi.Endpoints;

public static class UploadEndpoints
{
    public static void MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/upload").WithTags("Upload");

        group.MapPost("/avatar", async (
            IFormFile file,
            IFileStorageService storageService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userId = GetUserId(httpContext);
            if (userId is null)
                return Results.Unauthorized();

            try
            {
                var context = new FileUploadContext(userId.Value, "profile");
                var path = await storageService.SaveFileAsync(file, context, ct);
                return Results.Ok(new { success = true, path });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { success = false, message = ex.Message });
            }
        })
        .RequireAuthorization()
        .DisableAntiforgery();

        group.MapPost("/project-images", async (
            IFormFileCollection files,
            IFileStorageService storageService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userId = GetUserId(httpContext);
            if (userId is null)
                return Results.Unauthorized();

            try
            {
                var uploadContext = new FileUploadContext(userId.Value, "projects");
                var paths = await storageService.SaveFilesAsync(files, uploadContext, ct);

                return Results.Ok(new { success = true, paths });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { success = false, message = ex.Message });
            }
        })
        .RequireAuthorization()
        .DisableAntiforgery();
    }

    private static Guid? GetUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return null;

        return userId;
    }
}