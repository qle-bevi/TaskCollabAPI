using TaskCollab.Domain.Common;

namespace TaskCollab.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; private set; }
    public string Slug { get; private set; }
    public bool IsActive { get; private set; }
    public virtual ICollection<Workspace> Workspaces { get; private set; }
    public virtual ICollection<TenantUser> TenantUsers { get; private set; }
    
    private Tenant() : base()
    {
        Workspaces = new List<Workspace>();
        TenantUsers = new List<TenantUser>();
    }
    
    public Tenant(string name, string slug) : this()
    {
        Name = name;
        Slug = slug;
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}