using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Endpoints;

public static class ExperienceEndpoints
{
    public static void MapExperienceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles/{profileId:guid}/experiences").WithTags("Experiences");

        group.MapGet("/", async (
            Guid profileId,
            [FromServices] IExperienceService service,
            CancellationToken ct) =>
        {
            var experiences = await service.GetByProfileAsync(profileId, ct);
            return ApiResults.Ok(experiences, $"{experiences.Count()} experience(s) found");
        });

        group.MapGet("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IExperienceService service,
            CancellationToken ct) =>
        {
            var experience = await service.GetByIdAsync(id, ct);
            return ApiResults.Ok(experience);
        });

        group.MapPost("/", async (
            Guid profileId,
            CreateExperienceRequest req,
            [FromServices] IExperienceService service,
            CancellationToken ct) =>
        {
            var experience = await service.CreateAsync(profileId, req, ct);
            return ApiResults.Created(
                $"/api/profiles/{profileId}/experiences/{experience.Id}",
                experience,
                "Experience created successfully");
        })
        .RequireAuthorization();

        group.MapPut("/{id:guid}", async (
            Guid profileId,
            Guid id,
            UpdateExperienceRequest req,
            [FromServices] IExperienceService service,
            CancellationToken ct) =>
        {
            var experience = await service.UpdateAsync(profileId, id, req, ct);
            return ApiResults.Ok(experience, "Experience updated successfully");
        })
        .RequireAuthorization();

        group.MapDelete("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IExperienceService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(profileId, id, ct);
            return ApiResults.Ok("Experience deleted successfully");
        })
        .RequireAuthorization();
    }
}