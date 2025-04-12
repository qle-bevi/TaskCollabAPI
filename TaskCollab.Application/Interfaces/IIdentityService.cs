namespace TaskCollab.Application.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string UserId)> CreateUserAsync(string email, string password);
    Task<(bool Success, string Error)> DeleteUserAsync(string userId);
    Task<bool> CheckPasswordAsync(string userId, string password);
    Task<bool> UserExistsAsync(string email);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(bool Success, string Token)> LoginAsync(string email, string password, Guid? tenantId = null);
}