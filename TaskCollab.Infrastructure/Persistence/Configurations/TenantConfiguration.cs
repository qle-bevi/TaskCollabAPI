using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(t => t.Slug)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.HasIndex(t => t.Slug)
            .IsUnique();
            
        // Navigation properties
        builder.HasMany(t => t.Workspaces)
            .WithOne()
            .HasForeignKey(w => w.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(t => t.TenantUsers)
            .WithOne(tu => tu.Tenant)
            .HasForeignKey(tu => tu.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}