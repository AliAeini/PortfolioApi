using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class SkillCategoryConfiguration : BaseEntityConfiguration<SkillCategory>
{
    public override void Configure(EntityTypeBuilder<SkillCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("SkillCategories");

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(c => c.Name).IsUnique()
            .HasFilter("\"Name\" IS NOT NULL AND \"Status\" <> 5");

        builder.HasMany(c => c.Skills)
            .WithOne(s => s.SkillCategory)
            .HasForeignKey(s => s.SkillCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}