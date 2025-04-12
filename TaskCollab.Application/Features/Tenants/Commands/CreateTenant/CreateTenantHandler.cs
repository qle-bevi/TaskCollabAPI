using MediatR;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Entities;
using TaskCollab.Domain.Enums;
using TaskCollab.Domain.Interfaces;

namespace TaskCollab.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantUserRepository _tenantUserRepository;
    private readonly IIdentityService _identityService;

    public CreateTenantHandler(
        ITenantRepository tenantRepository, 
        IUserRepository userRepository, 
        ITenantUserRepository tenantUserRepository,
        IIdentityService identityService)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _tenantUserRepository = tenantUserRepository;
        _identityService = identityService;
    }

    public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // 1. Créer un nouveau tenant
        var tenant = new Tenant(request.Name, request.Slug);
        await _tenantRepository.AddAsync(tenant);

        // 2. Créer un nouvel utilisateur (owner)
        var (success, userId) = await _identityService.CreateUserAsync(request.OwnerEmail, request.Password);
        if (!success)
        {
            throw new ApplicationException("Échec de la création de l'utilisateur.");
        }
        
        var user = new User(request.OwnerEmail, request.OwnerFirstName, request.OwnerLastName);
        await _userRepository.AddAsync(user);

        // 3. Associer l'utilisateur au tenant en tant que propriétaire
        var tenantUser = new TenantUser(user.Id, tenant.Id, TenantRole.Owner);
        tenantUser.AcceptInvitation(); // Le créateur accepte automatiquement l'invitation
        await _tenantUserRepository.AddAsync(tenantUser);

        // 4. Générer un token JWT pour la première connexion
        var (tokenSuccess, token) = await _identityService.LoginAsync(request.OwnerEmail, request.Password, tenant.Id);
        if (!tokenSuccess)
        {
            throw new ApplicationException("Échec de la génération du token de connexion.");
        }

        // 5. Retourner la réponse
        return new CreateTenantResponse
        {
            TenantId = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            OwnerId = user.Id,
            OwnerEmail = user.Email,
            Token = token
        };
    }
}