using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskCollab.Domain.Common;
using TaskCollab.Domain.Interfaces;
using TaskCollab.Infrastructure.Persistence;

namespace TaskCollab.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    
    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    
    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }
    
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<T>().AnyAsync(e => e.Id == id);
    }
}