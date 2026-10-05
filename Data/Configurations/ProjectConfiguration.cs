using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProjectConfiguration : BaseEntityConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);

        builder.ToTable("Projects");

        builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(5000);
        builder.Property(p => p.ShortDescription).HasMaxLength(500);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);
        builder.Property(p => p.GithubUrl).HasMaxLength(500);
        builder.Property(p => p.LiveUrl).HasMaxLength(500);
        builder.Property(p => p.TechStack).HasMaxLength(1000);
        builder.Property(p => p.DisplayOrder).HasDefaultValue(0);
        builder.Property(p => p.IsFeatured).HasDefaultValue(false);

        builder.HasIndex(p => p.ProjectCategoryId);
        builder.HasIndex(p => p.IsFeatured);
    }
}