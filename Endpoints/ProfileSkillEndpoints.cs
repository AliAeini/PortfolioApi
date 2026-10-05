using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Endpoints;

public static class ProfileSkillEndpoints
{
    public static void MapProfileSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profiles/{profileId:guid}/skills")
                       .WithTags("Profile Skills");

        group.MapGet("/", async (
            Guid profileId,
            [FromServices] IProfileSkillService service,
            CancellationToken ct) =>
        {
            var skills = await service.GetByProfileAsync(profileId, ct);
            return ApiResults.Ok(skills, $"{skills.Count()} skill(s) found");
        });

        group.MapPost("/", async (
            Guid profileId,
            AddProfileSkillRequest req,
            [FromServices] IProfileSkillService service,
            CancellationToken ct) =>
        {
            var profileSkill = await service.AddAsync(profileId, req, ct);
            return ApiResults.Created(
                $"/api/profiles/{profileId}/skills/{profileSkill.Id}",
                profileSkill,
                "Skill added to profile successfully");
        })
        .RequireAuthorization(policy => policy.RequireRole("Owner", "Admin"));

        group.MapPut("/{id:guid}", async (
            Guid profileId,
            Guid id,
            UpdateProfileSkillRequest req,
            [FromServices] IProfileSkillService service,
            CancellationToken ct) =>
        {
            var profileSkill = await service.UpdateAsync(profileId, id, req, ct);
            return ApiResults.Ok(profileSkill, "Profile skill updated successfully");
        })
        .RequireAuthorization(policy => policy.RequireRole("Owner", "Admin"));

        group.MapDelete("/{id:guid}", async (
            Guid profileId,
            Guid id,
            [FromServices] IProfileSkillService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(profileId, id, ct);
            return ApiResults.Ok("Skill removed from profile successfully");
        })
        .RequireAuthorization(policy => policy.RequireRole("Owner", "Admin"));
    }
}