namespace TaskCollab.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantResponse
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerEmail { get; set; }
    public string Token { get; set; }
}