using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(t => t.Description)
            .HasMaxLength(2000);
            
        builder.Property(t => t.Priority)
            .IsRequired();
            
        builder.Property(t => t.Status)
            .IsRequired();
            
        // Filtre de requête global pour le multi-tenant
        builder.HasQueryFilter(t => t.TenantId == null || 
                                    t.TenantId == Guid.Empty || 
                                    t.TenantId == GetCurrentTenantId());
            
        // Navigation properties
        builder.HasOne(t => t.AssignedToUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
    
    // Cette méthode sera remplacée lors de l'exécution
    private static Guid GetCurrentTenantId() => Guid.Empty;
}