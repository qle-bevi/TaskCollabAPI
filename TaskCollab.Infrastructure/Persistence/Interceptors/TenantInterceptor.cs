using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskCollab.Application.Interfaces;
using TaskCollab.Domain.Common;

namespace TaskCollab.Infrastructure.Persistence.Interceptors;

public class TenantInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService? _currentUserService;

    public TenantInterceptor(ICurrentUserService? currentUserService)
    {
        _currentUserService = currentUserService;
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyTenantId(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyTenantId(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyTenantId(DbContext? context)
    {
        if (context == null || _currentUserService == null || _currentUserService.TenantId == null)
            return;

        // Appliquer le TenantId à toutes les entités qui sont des BaseTenantEntity
        foreach (var entry in context.ChangeTracker.Entries<BaseTenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.GetType().GetProperty("TenantId")?.SetValue(entry.Entity, _currentUserService.TenantId);
            }
        }
    }
}