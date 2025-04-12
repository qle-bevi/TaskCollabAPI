using MediatR;

namespace TaskCollab.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string TenantSlug { get; set; }
}