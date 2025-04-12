using TaskCollab.Domain.Common;
using TaskCollab.Domain.Enums;
using TaskStatus = TaskCollab.Domain.Enums.TaskStatus;

namespace TaskCollab.Domain.Entities;

public class TaskItem : BaseTenantEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskPriority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime? DueDate { get; private set; }
    
    public virtual Project Project { get; private set; }
    public virtual User AssignedToUser { get; private set; }
    public virtual User CreatedByUser { get; private set; }
    
    private TaskItem() : base()
    {
    }
    
    public TaskItem(
        Guid tenantId,
        Guid projectId,
        string title,
        string description,
        TaskPriority priority,
        Guid createdByUserId,
        DateTime? dueDate = null,
        Guid? assignedToUserId = null) : base(tenantId)
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        Priority = priority;
        Status = TaskStatus.Todo;
        CreatedByUserId = createdByUserId;
        DueDate = dueDate;
        AssignedToUserId = assignedToUserId;
    }
    
    public void Update(string title, string description, TaskPriority priority, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AssignToUser(Guid userId)
    {
        AssignedToUserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Unassign()
    {
        AssignedToUserId = null;
        UpdatedAt = DateTime.UtcNow;
    }
}