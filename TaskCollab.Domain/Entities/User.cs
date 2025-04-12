using TaskCollab.Domain.Common;

namespace TaskCollab.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsActive { get; private set; }
    public virtual ICollection<TenantUser> TenantUsers { get; private set; }
    
    private User() : base()
    {
        TenantUsers = new List<TenantUser>();
    }
    
    public User(string email, string firstName, string lastName) : this()
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
    }
    
    public string FullName => $"{FirstName} {LastName}";
    
    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
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
}