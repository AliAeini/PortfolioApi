using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class SkillConfiguration : BaseEntityConfiguration<Skill>
{
    public override void Configure(EntityTypeBuilder<Skill> builder)
    {
        base.Configure(builder);

        builder.ToTable("Skills");

        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Level).IsRequired();
        builder.Property(s => s.DisplayOrder).HasDefaultValue(0);
        builder.Property(s => s.IconUrl).HasMaxLength(500);

        builder.HasIndex(s => s.SkillCategoryId);
    }
}