namespace TaskCollab.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    bool IsAuthenticated { get; }
}