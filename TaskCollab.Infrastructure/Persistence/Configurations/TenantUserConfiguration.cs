using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
{
    public void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        builder.HasKey(tu => tu.Id);
        
        builder.Property(tu => tu.Role)
            .IsRequired();
            
        builder.Property(tu => tu.HasAcceptedInvitation)
            .IsRequired();
            
        // Index pour accélérer les recherches par utilisateur et par tenant
        builder.HasIndex(tu => new { tu.UserId, tu.TenantId })
            .IsUnique();
    }
}