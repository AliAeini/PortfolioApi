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

        builder.Property(s => s.Platform).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Url).IsRequired().HasMaxLength(500);
        builder.Property(s => s.IconUrl).HasMaxLength(500);
        builder.Property(s => s.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(s => s.ProfileId);
    }
}