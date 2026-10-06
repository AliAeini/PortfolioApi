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
        builder.Property(p => p.GithubUrl).HasMaxLength(500);
        builder.Property(p => p.LiveUrl).HasMaxLength(500);
        builder.Property(p => p.IsFeatured).HasDefaultValue(false);
        builder.Property(p => p.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(p => p.Profile)
            .WithMany(pr => pr.Projects)
            .HasForeignKey(p => p.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ProjectCategory)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ProjectCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.ProfileId);
        builder.HasIndex(p => p.ProjectCategoryId);
        builder.HasIndex(p => p.IsFeatured);
    }
}