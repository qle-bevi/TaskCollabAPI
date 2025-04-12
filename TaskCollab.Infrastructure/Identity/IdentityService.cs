using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskCollab.Application.Interfaces;

namespace TaskCollab.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<(bool Success, string UserId)> CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.Succeeded, user.Id);
    }

    public async Task<(bool Success, string Error)> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return (false, "Utilisateur non trouvé.");
        }

        var result = await _userManager.DeleteAsync(user);

        return (result.Succeeded, result.Succeeded ? string.Empty : string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<bool> CheckPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        // Implémentation simplifiée - à adapter selon vos besoins
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        // Ici, vous pourriez implémenter une logique plus avancée basée sur les Claims
        return true;
    }

    public async Task<(bool Success, string Token)> LoginAsync(string email, string password, Guid? tenantId = null)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return (false, string.Empty);
        }

        var result = await _userManager.CheckPasswordAsync(user, password);

        if (!result)
        {
            return (false, string.Empty);
        }

        var token = GenerateJwtToken(user, tenantId);

        return (true, token);
    }

    private string GenerateJwtToken(ApplicationUser user, Guid? tenantId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", user.Id)
        };

        if (tenantId.HasValue && tenantId.Value != Guid.Empty)
        {
            claims.Add(new Claim("tid", tenantId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryMinutes));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}