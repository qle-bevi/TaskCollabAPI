using TaskCollab.Domain.Entities;

namespace TaskCollab.Domain.Interfaces;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<Tenant> GetBySlugAsync(string slug);
    Task<bool> ExistsBySlugAsync(string slug);
}