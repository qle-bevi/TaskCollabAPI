using TaskCollab.Domain.Common;

namespace TaskCollab.Domain.Entities;

public class Workspace : BaseTenantEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public virtual ICollection<Project> Projects { get; private set; }
    
    private Workspace() : base()
    {
        Projects = new List<Project>();
    }
    
    public Workspace(Guid tenantId, string name, string description) : base(tenantId)
    {
        Name = name;
        Description = description;
        Projects = new List<Project>();
    }
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}