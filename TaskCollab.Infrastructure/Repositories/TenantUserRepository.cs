using Microsoft.EntityFrameworkCore;
using TaskCollab.Domain.Entities;
using TaskCollab.Domain.Interfaces;
using TaskCollab.Infrastructure.Persistence;

namespace TaskCollab.Infrastructure.Repositories;

public class TenantUserRepository : RepositoryBase<TenantUser>, ITenantUserRepository
{
    public TenantUserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<TenantUser> GetByUserAndTenantAsync(Guid userId, Guid tenantId)
    {
        return await _context.TenantUsers
            .FirstOrDefaultAsync(tu => tu.UserId == userId && tu.TenantId == tenantId);
    }
    
    public async Task<IReadOnlyList<TenantUser>> GetByTenantAsync(Guid tenantId)
    {
        return await _context.TenantUsers
            .Where(tu => tu.TenantId == tenantId)
            .Include(tu => tu.User)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<TenantUser>> GetByUserAsync(Guid userId)
    {
        return await _context.TenantUsers
            .Where(tu => tu.UserId == userId)
            .Include(tu => tu.Tenant)
            .ToListAsync();
    }
    
    public async Task<bool> ExistsByUserAndTenantAsync(Guid userId, Guid tenantId)
    {
        return await _context.TenantUsers
            .AnyAsync(tu => tu.UserId == userId && tu.TenantId == tenantId);
    }
}