using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProjectCategoryConfiguration : BaseEntityConfiguration<ProjectCategory>
{
    public override void Configure(EntityTypeBuilder<ProjectCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProjectCategories");

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(c => c.Name).IsUnique()
            .HasFilter("\"Name\" IS NOT NULL AND \"Status\" <> 5");

        builder.HasMany(c => c.Projects)
            .WithOne(p => p.ProjectCategory)
            .HasForeignKey(p => p.ProjectCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}