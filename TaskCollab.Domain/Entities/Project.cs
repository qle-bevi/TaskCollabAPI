using TaskCollab.Domain.Common;
using TaskCollab.Domain.Enums;

namespace TaskCollab.Domain.Entities;

public class Project : BaseTenantEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid WorkspaceId { get; private set; }
    public ProjectStatus Status { get; private set; }
    
    public virtual Workspace Workspace { get; private set; }
    public virtual ICollection<TaskItem> Tasks { get; private set; }
    
    private Project() : base()
    {
        Tasks = new List<TaskItem>();
    }
    
    public Project(Guid tenantId, Guid workspaceId, string name, string description) : base(tenantId)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
        Status = ProjectStatus.Active;
        Tasks = new List<TaskItem>();
    }
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeStatus(ProjectStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}