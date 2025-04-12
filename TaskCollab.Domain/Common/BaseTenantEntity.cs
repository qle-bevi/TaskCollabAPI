namespace TaskCollab.Domain.Common;

public abstract class BaseTenantEntity : BaseEntity
{
    public Guid TenantId { get; protected set; }
    
    protected BaseTenantEntity(Guid tenantId) : base()
    {
        TenantId = tenantId;
    }
    
    // Pour EF Core
    protected BaseTenantEntity() : base()
    {
    }
}