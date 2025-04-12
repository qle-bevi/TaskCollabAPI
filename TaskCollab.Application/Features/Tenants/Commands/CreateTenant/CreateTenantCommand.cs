using MediatR;

namespace TaskCollab.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommand : IRequest<CreateTenantResponse>
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string OwnerEmail { get; set; }
    public string OwnerFirstName { get; set; }
    public string OwnerLastName { get; set; }
    public string Password { get; set; }
}