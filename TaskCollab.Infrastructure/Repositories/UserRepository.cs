using Microsoft.EntityFrameworkCore;
using TaskCollab.Domain.Entities;
using TaskCollab.Domain.Interfaces;
using TaskCollab.Infrastructure.Persistence;

namespace TaskCollab.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}