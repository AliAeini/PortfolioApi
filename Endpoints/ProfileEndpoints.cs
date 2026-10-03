using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Services;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace PortfolioApi.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles")
                       .WithTags("Profiles")
                       .AddFluentValidationAutoValidation();

        group.MapGet("/", async (
            [FromServices] IProfileService service,
            CancellationToken ct) =>
        {
            var profiles = await service.GetAllAsync(ct);
            return ApiResults.Ok(profiles, $"{profiles.Count()} profile(s) found");
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            [FromServices] IProfileService service,
            CancellationToken ct) =>
        {
            var profile = await service.GetByIdAsync(id, ct);
            return ApiResults.Ok(profile);
        });

        group.MapPost("/", async (
            CreateProfileRequest req,
            [FromServices] IProfileService service,
            CancellationToken ct) =>
        {
            var profile = await service.CreateAsync(req, ct);
            return ApiResults.Created($"/api/profiles/{profile.Id}", profile,
                                      "Profile created successfully");
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateProfileRequest req,
            [FromServices] IProfileService service,
            CancellationToken ct) =>
        {
            var profile = await service.UpdateAsync(id, req, ct);
            return ApiResults.Ok(profile, "Profile updated successfully");
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            [FromServices] IProfileService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return ApiResults.Ok("Profile deleted successfully");
        });
    }
}