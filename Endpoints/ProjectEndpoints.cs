using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles/{profileId:guid}/projects").WithTags("Projects");

        group.MapGet("/", async (
            Guid profileId,
            [FromServices] IProjectService service,
            CancellationToken ct) =>
        {
            var projects = await service.GetByProfileAsync(profileId, ct);
            return ApiResults.Ok(projects, $"{projects.Count()} project(s) found");
        });

        group.MapGet("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IProjectService service,
            CancellationToken ct) =>
        {
            var project = await service.GetByIdAsync(id, ct);
            return ApiResults.Ok(project);
        });

        group.MapPost("/", async (
            Guid profileId,
            CreateProjectRequest req,
            [FromServices] IProjectService service,
            CancellationToken ct) =>
        {
            var project = await service.CreateAsync(profileId, req, ct);
            return ApiResults.Created(
                $"/api/profiles/{profileId}/projects/{project.Id}",
                project,
                "Project created successfully");
        })
        .RequireAuthorization();

        group.MapPut("/{id:guid}", async (
            Guid profileId,
            Guid id,
            UpdateProjectRequest req,
            [FromServices] IProjectService service,
            CancellationToken ct) =>
        {
            var project = await service.UpdateAsync(profileId, id, req, ct);
            return ApiResults.Ok(project, "Project updated successfully");
        })
        .RequireAuthorization();

        group.MapDelete("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IProjectService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(profileId, id, ct);
            return ApiResults.Ok("Project deleted successfully");
        })
        .RequireAuthorization();
    }
}