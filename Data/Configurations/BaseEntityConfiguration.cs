using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Common;

namespace PortfolioApi.Data.Configurations;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.RowId).ValueGeneratedOnAdd().UseIdentityByDefaultColumn();

        builder.Property(e => e.RevSeq).HasDefaultValue(1).IsRequired();

        builder.Property(e => e.CreatedAt).IsRequired();

        builder.Property(e => e.UpdatedAt).IsRequired(false);

        builder.Property(e => e.CreatedByUserId).IsRequired(false);

        builder.Property(e => e.Status).HasConversion<int>().IsRequired();

        builder.Property(e => e.RowVersion).HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

        builder.HasQueryFilter(e => e.Status != EntityStatus.Deleted);
    }
}