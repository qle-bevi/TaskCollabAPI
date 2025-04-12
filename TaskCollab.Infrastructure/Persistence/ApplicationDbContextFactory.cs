using Microsoft.EntityFrameworkCore;
using TaskCollab.Infrastructure.Persistence.Interceptors;

namespace TaskCollab.Infrastructure.Persistence;

public class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
{
    protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
    {
        // Création de mock interceptors pour le design-time
        var tenantInterceptor = new TenantInterceptor(null);
        var auditableEntityInterceptor = new AuditableEntityInterceptor(null);
        
        return new ApplicationDbContext(options, tenantInterceptor, auditableEntityInterceptor);
    }
}