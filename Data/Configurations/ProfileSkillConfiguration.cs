using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProfileSkillConfiguration : BaseEntityConfiguration<ProfileSkill>
{
    public override void Configure(EntityTypeBuilder<ProfileSkill> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProfileSkills");

        builder.Property(ps => ps.Level).IsRequired();
        builder.Property(ps => ps.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(ps => ps.Profile)
            .WithMany(p => p.ProfileSkills)
            .HasForeignKey(ps => ps.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.Skill)
            .WithMany(s => s.ProfileSkills)
            .HasForeignKey(ps => ps.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ps => new { ps.ProfileId, ps.SkillId })
            .IsUnique()
            .HasFilter("\"Status\" <> 5");
    }
}