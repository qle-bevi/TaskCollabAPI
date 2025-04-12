using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskCollab.Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Personnaliser les noms de tables pour éviter les conflits avec d'autres tables
        builder.Entity<ApplicationUser>().ToTable("IdentityUsers");
    }
}