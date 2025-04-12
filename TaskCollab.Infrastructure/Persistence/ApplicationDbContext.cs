using Microsoft.EntityFrameworkCore;
using TaskCollab.Domain.Entities;
using TaskCollab.Application.Interfaces;
using TaskCollab.Infrastructure.Persistence.Interceptors;

namespace TaskCollab.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly TenantInterceptor _tenantInterceptor;
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        TenantInterceptor tenantInterceptor,
        AuditableEntityInterceptor auditableEntityInterceptor) : base(options)
    {
        _tenantInterceptor = tenantInterceptor;
        _auditableEntityInterceptor = auditableEntityInterceptor;
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_tenantInterceptor, _auditableEntityInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}