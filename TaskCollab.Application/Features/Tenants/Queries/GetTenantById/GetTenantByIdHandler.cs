using MediatR;
using TaskCollab.Application.Common.Exceptions;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Interfaces;

namespace TaskCollab.Application.Features.Tenants.Queries.GetTenantById;

public class GetTenantByIdHandler : IRequestHandler<GetTenantByIdQuery, TenantDto>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTenantByIdHandler(ITenantRepository tenantRepository, ICurrentUserService currentUserService)
    {
        _tenantRepository = tenantRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TenantDto> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);
        if (tenant == null)
        {
            throw new NotFoundException("Tenant", request.Id);
        }

        // Vérification d'autorisation simple - on pourrait utiliser un service d'autorisation plus élaboré
        if (_currentUserService.TenantId != tenant.Id)
        {
            throw new UnauthorizedAccessException("Vous n'avez pas accès à ce tenant.");
        }

        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            IsActive = tenant.IsActive,
            CreatedAt = tenant.CreatedAt,
            UpdatedAt = tenant.UpdatedAt
        };
    }
}