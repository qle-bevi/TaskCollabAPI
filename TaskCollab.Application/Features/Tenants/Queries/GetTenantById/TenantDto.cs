namespace TaskCollab.Application.Features.Tenants.Queries.GetTenantById;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}