using TaskCollab.Domain.Entities;

namespace TaskCollab.Domain.Interfaces;

public interface ITenantUserRepository : IRepositoryBase<TenantUser>
{
    Task<TenantUser> GetByUserAndTenantAsync(Guid userId, Guid tenantId);
    Task<IReadOnlyList<TenantUser>> GetByTenantAsync(Guid tenantId);
    Task<IReadOnlyList<TenantUser>> GetByUserAsync(Guid userId);
    Task<bool> ExistsByUserAndTenantAsync(Guid userId, Guid tenantId);
}