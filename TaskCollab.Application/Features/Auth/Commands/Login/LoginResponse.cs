namespace TaskCollab.Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; }
    public string TenantSlug { get; set; }
}