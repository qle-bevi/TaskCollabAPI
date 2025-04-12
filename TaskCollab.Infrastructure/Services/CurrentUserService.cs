using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskCollab.Application.Interfaces;

namespace TaskCollab.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("tid");
            return !string.IsNullOrEmpty(tenantId) ? Guid.Parse(tenantId) : null;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}