namespace PortfolioApi.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public long RowId { get; private set; }
    public int RevSeq { get; private set; } = 1;
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Guid? CreatedByUserId { get; protected set; }
    public EntityStatus Status { get; protected set; }
 public uint RowVersion { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = EntityStatus.Pending;
    }

    protected BaseEntity(Guid? createdByUserId) : this()
    {
        CreatedByUserId = createdByUserId;
    }

    public void IncreaseRevision() => RevSeq++;

    public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;

    public void SetCreatedBy(Guid? userId) => CreatedByUserId = userId;

    public void SetStatus(EntityStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}