using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Name)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(w => w.Description)
            .HasMaxLength(500);
            
        // Filtre de requête global pour le multi-tenant
        builder.HasQueryFilter(w => w.TenantId == null || 
                                    w.TenantId == Guid.Empty || 
                                    w.TenantId == GetCurrentTenantId());
            
        // Navigation properties
        builder.HasMany(w => w.Projects)
            .WithOne(p => p.Workspace)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    // Cette méthode sera remplacée lors de l'exécution
    private static Guid GetCurrentTenantId() => Guid.Empty;
}