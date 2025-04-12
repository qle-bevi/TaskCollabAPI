using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskCollab.Domain.Entities;

namespace TaskCollab.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .HasMaxLength(320)
            .IsRequired();
            
        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.HasIndex(u => u.Email)
            .IsUnique();
            
        // Navigation properties
        builder.HasMany(u => u.TenantUsers)
            .WithOne(tu => tu.User)
            .HasForeignKey(tu => tu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}