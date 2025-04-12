using Microsoft.EntityFrameworkCore;
using TaskCollab.Domain.Entities;
using TaskCollab.Domain.Interfaces;
using TaskCollab.Infrastructure.Persistence;

namespace TaskCollab.Infrastructure.Repositories;

public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Tenant> GetBySlugAsync(string slug)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug);
    }
    
    public async Task<bool> ExistsBySlugAsync(string slug)
    {
        return await _context.Tenants
            .AnyAsync(t => t.Slug == slug);
    }
}