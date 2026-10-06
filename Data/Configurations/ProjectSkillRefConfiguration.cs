using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProjectSkillRefConfiguration : BaseEntityConfiguration<ProjectSkillRef>
{
    public override void Configure(EntityTypeBuilder<ProjectSkillRef> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProjectSkillRefs");

        builder.Ignore(ps => ps.RowVersion);

        builder.Property(ps => ps.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(ps => ps.Project)
            .WithMany(p => p.ProjectSkills)
            .HasForeignKey(ps => ps.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.ProfileSkill)
            .WithMany(p => p.ProjectSkillRefs)
            .HasForeignKey(ps => ps.ProfileSkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ps => new { ps.ProjectId, ps.ProfileSkillId })
            .IsUnique()
            .HasFilter("\"Status\" <> 5");
    }
}