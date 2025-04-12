using TaskCollab.Domain.Enums;
using TaskCollab.Domain.Common;

namespace TaskCollab.Domain.Entities;

public class TenantUser : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid TenantId { get; private set; }
    public TenantRole Role { get; private set; }
    public bool HasAcceptedInvitation { get; private set; }
    public DateTime? InvitationAcceptedAt { get; private set; }
    
    public virtual User User { get; private set; }
    public virtual Tenant Tenant { get; private set; }
    
    private TenantUser() : base()
    {
    }
    
    public TenantUser(Guid userId, Guid tenantId, TenantRole role) : this()
    {
        UserId = userId;
        TenantId = tenantId;
        Role = role;
        HasAcceptedInvitation = false;
    }
    
    public void AcceptInvitation()
    {
        HasAcceptedInvitation = true;
        InvitationAcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeRole(TenantRole newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }
}