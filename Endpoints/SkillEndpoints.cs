using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Endpoints;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/skills").WithTags("Skills");

        group.MapGet("/", async (
            [FromServices] ISkillService service,
            CancellationToken ct) =>
        {
            var skills = await service.GetAllAsync(ct);
            return ApiResults.Ok(skills, $"{skills.Count()} skill(s) found");
        });

        group.MapGet("/grouped", async (
            [FromServices] ISkillService service,
            CancellationToken ct) =>
        {
            var grouped = await service.GetGroupedAsync(ct);
            return ApiResults.Ok(grouped, $"{grouped.Count()} categor(ies) found");
        });

        group.MapGet("/categories", async (
            [FromServices] ISkillService service,
            CancellationToken ct) =>
        {
            var categories = await service.GetCategoriesAsync(ct);
            return ApiResults.Ok(categories, $"{categories.Count()} categor(ies) found");
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            [FromServices] ISkillService service,
            CancellationToken ct) =>
        {
            var skill = await service.GetByIdAsync(id, ct);
            return ApiResults.Ok(skill);
        });
    }
}