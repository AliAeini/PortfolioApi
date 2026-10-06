using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class SocialLinkConfiguration : BaseEntityConfiguration<SocialLink>
{
    public override void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        base.Configure(builder);

        builder.ToTable("SocialLinks");

        builder.Property(s => s.Platform)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.Url).IsRequired().HasMaxLength(500);
        builder.Property(s => s.IconUrl).HasMaxLength(500);
        builder.Property(s => s.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(s => s.Profile)
            .WithMany(p => p.SocialLinks)
            .HasForeignKey(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProfileId);

        builder.HasIndex(s => new { s.ProfileId, s.Platform })
            .IsUnique()
            .HasFilter("\"Status\" <> 5");
    }
}