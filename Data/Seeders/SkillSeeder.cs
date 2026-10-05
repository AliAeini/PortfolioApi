using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Seeders;

public class SkillSeeder : ISeeder
{
    private readonly AppDbContext _db;
    private readonly ILogger<SkillSeeder> _logger;

    public SkillSeeder(AppDbContext db, ILogger<SkillSeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public int Order => 15;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Skills.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Skills already exist. Skipping seed.");
            return;
        }

        var skills = new[]
        {
            // ===== Frontend =====
            Skill.Create("HTML",                    95, SkillCategorySeeder.FrontendId, 1),
            Skill.Create("CSS",                     90, SkillCategorySeeder.FrontendId, 2),
            Skill.Create("JavaScript",              90, SkillCategorySeeder.FrontendId, 3),
            Skill.Create("TypeScript",              85, SkillCategorySeeder.FrontendId, 4),
            Skill.Create("React",                   88, SkillCategorySeeder.FrontendId, 5),
            Skill.Create("Next.js",                 85, SkillCategorySeeder.FrontendId, 6),
            Skill.Create("Tailwind CSS",            90, SkillCategorySeeder.FrontendId, 7),
            Skill.Create("Vue.js",                  70, SkillCategorySeeder.FrontendId, 8),

            // ===== Backend =====
            Skill.Create("C#",                      90, SkillCategorySeeder.BackendId,  1),
            Skill.Create(".NET",                    88, SkillCategorySeeder.BackendId,  2),
            Skill.Create("ASP.NET Core",            85, SkillCategorySeeder.BackendId,  3),
            Skill.Create("Node.js",                 80, SkillCategorySeeder.BackendId,  4),
            Skill.Create("Express.js",              75, SkillCategorySeeder.BackendId,  5),
            Skill.Create("Python",                  70, SkillCategorySeeder.BackendId,  6),
            Skill.Create("REST API",                90, SkillCategorySeeder.BackendId,  7),

            // ===== Database =====
            Skill.Create("PostgreSQL",              85, SkillCategorySeeder.DatabaseId, 1),
            Skill.Create("SQL Server",              80, SkillCategorySeeder.DatabaseId, 2),
            Skill.Create("MongoDB",                 70, SkillCategorySeeder.DatabaseId, 3),
            Skill.Create("Redis",                   65, SkillCategorySeeder.DatabaseId, 4),
            Skill.Create("Entity Framework Core",   88, SkillCategorySeeder.DatabaseId, 5),

            // ===== DevOps =====
            Skill.Create("Docker",                  75, SkillCategorySeeder.DevOpsId,   1),
            Skill.Create("Git",                     90, SkillCategorySeeder.DevOpsId,   2),
            Skill.Create("GitHub Actions",          70, SkillCategorySeeder.DevOpsId,   3),
            Skill.Create("CI/CD",                   70, SkillCategorySeeder.DevOpsId,   4),
            Skill.Create("Linux",                   75, SkillCategorySeeder.DevOpsId,   5),

            // ===== Mobile =====
            Skill.Create("React Native",            65, SkillCategorySeeder.MobileId,   1),
            Skill.Create("Flutter",                 50, SkillCategorySeeder.MobileId,   2),

            // ===== Tools =====
            Skill.Create("VS Code",                 95, SkillCategorySeeder.ToolsId,    1),
            Skill.Create("Visual Studio",           85, SkillCategorySeeder.ToolsId,    2),
            Skill.Create("Postman",                 85, SkillCategorySeeder.ToolsId,    3),
            Skill.Create("Figma",                   70, SkillCategorySeeder.ToolsId,    4),

            // ===== Soft Skills =====
            Skill.Create("Problem Solving",         90, SkillCategorySeeder.SoftSkillsId, 1),
            Skill.Create("Team Work",               90, SkillCategorySeeder.SoftSkillsId, 2),
            Skill.Create("Communication",           85, SkillCategorySeeder.SoftSkillsId, 3),
            Skill.Create("Time Management",         80, SkillCategorySeeder.SoftSkillsId, 4),

            // ===== Testing =====
            Skill.Create("xUnit",                   75, SkillCategorySeeder.TestingId,  1),
            Skill.Create("Jest",                    70, SkillCategorySeeder.TestingId,  2),
            Skill.Create("Playwright",              65, SkillCategorySeeder.TestingId,  3),

            // ===== Security =====
            Skill.Create("JWT",                     85, SkillCategorySeeder.SecurityId, 1),
            Skill.Create("OAuth2",                  75, SkillCategorySeeder.SecurityId, 2),
            Skill.Create("HTTPS/TLS",               70, SkillCategorySeeder.SecurityId, 3),

            // ===== Cloud =====
            Skill.Create("AWS",                     65, SkillCategorySeeder.CloudId,    1),
            Skill.Create("Azure",                   60, SkillCategorySeeder.CloudId,    2),
            Skill.Create("Vercel",                  75, SkillCategorySeeder.CloudId,    3),
        };

        _db.Skills.AddRange(skills);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} skills.", skills.Length);
    }
}