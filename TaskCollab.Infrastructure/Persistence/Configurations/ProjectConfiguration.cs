using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(p => p.Description)
            .HasMaxLength(500);
            
        builder.Property(p => p.Status)
            .IsRequired();
            
        // Filtre de requête global pour le multi-tenant
        builder.HasQueryFilter(p => p.TenantId == null || 
                                    p.TenantId == Guid.Empty || 
                                    p.TenantId == GetCurrentTenantId());
            
        // Navigation properties
        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    // Cette méthode sera remplacée lors de l'exécution
    private static Guid GetCurrentTenantId() => Guid.Empty;
}