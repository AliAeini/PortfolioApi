using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Endpoints;

public static class EducationEndpoints
{
    public static void MapEducationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles/{profileId:guid}/educations").WithTags("Educations");

        group.MapGet("/", async (
            Guid profileId,
            [FromServices] IEducationService service,
            CancellationToken ct) =>
        {
            var educations = await service.GetByProfileAsync(profileId, ct);
            return ApiResults.Ok(educations, $"{educations.Count()} education(s) found");
        });

        group.MapGet("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IEducationService service,
            CancellationToken ct) =>
        {
            var education = await service.GetByIdAsync(id, ct);
            return ApiResults.Ok(education);
        });

        group.MapPost("/", async (
            Guid profileId,
            CreateEducationRequest req,
            [FromServices] IEducationService service,
            CancellationToken ct) =>
        {
            var education = await service.CreateAsync(profileId, req, ct);
            return ApiResults.Created(
                $"/api/profiles/{profileId}/educations/{education.Id}",
                education,
                "Education created successfully");
        })
        .RequireAuthorization();

        group.MapPut("/{id:guid}", async (
            Guid profileId,
            Guid id,
            UpdateEducationRequest req,
            [FromServices] IEducationService service,
            CancellationToken ct) =>
        {
            var education = await service.UpdateAsync(profileId, id, req, ct);
            return ApiResults.Ok(education, "Education updated successfully");
        })
        .RequireAuthorization();

        group.MapDelete("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IEducationService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(profileId, id, ct);
            return ApiResults.Ok("Education deleted successfully");
        })
        .RequireAuthorization();
    }
}