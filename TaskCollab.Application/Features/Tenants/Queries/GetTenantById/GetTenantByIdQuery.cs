using MediatR;

namespace TaskCollab.Application.Features.Tenants.Queries.GetTenantById;

public class GetTenantByIdQuery : IRequest<TenantDto>
{
    public Guid Id { get; set; }
}