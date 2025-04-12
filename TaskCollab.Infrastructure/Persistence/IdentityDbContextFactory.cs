using Microsoft.EntityFrameworkCore;
using TaskCollab.Infrastructure.Persistence;

namespace TaskCollab.Infrastructure.Identity;

public class IdentityDbContextFactory : DesignTimeDbContextFactoryBase<IdentityDbContext>
{
    protected override IdentityDbContext CreateNewInstance(DbContextOptions<IdentityDbContext> options)
    {
        return new IdentityDbContext(options);
    }
}