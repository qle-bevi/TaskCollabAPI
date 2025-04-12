using MediatR;
using TaskCollab.Application.Common.Exceptions;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Interfaces;

namespace TaskCollab.Application.Features.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantUserRepository _tenantUserRepository;
    private readonly IIdentityService _identityService;

    public LoginHandler(
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

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Récupérer le tenant via son slug
        var tenant = await _tenantRepository.GetBySlugAsync(request.TenantSlug);
        if (tenant == null)
        {
            throw new NotFoundException($"L'organisation avec l'identifiant '{request.TenantSlug}' n'existe pas.");
        }

        // 2. Récupérer l'utilisateur via son email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
        }

        // 3. Vérifier que l'utilisateur a accès à ce tenant
        var tenantUser = await _tenantUserRepository.GetByUserAndTenantAsync(user.Id, tenant.Id);
        if (tenantUser == null)
        {
            throw new UnauthorizedAccessException("Vous n'avez pas accès à cette organisation.");
        }

        // 4. Vérifier que l'utilisateur a accepté l'invitation
        if (!tenantUser.HasAcceptedInvitation)
        {
            throw new UnauthorizedAccessException("Vous devez d'abord accepter l'invitation pour vous connecter.");
        }

        // 5. Vérifier les identifiants
        var (success, token) = await _identityService.LoginAsync(request.Email, request.Password, tenant.Id);
        if (!success)
        {
            throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
        }

        // 6. Retourner la réponse
        return new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            TenantId = tenant.Id,
            TenantName = tenant.Name,
            TenantSlug = tenant.Slug
        };
    }
}