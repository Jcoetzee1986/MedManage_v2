using MedManage.Core.Entities;
using MedManage.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedManage.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation with multi-tenant support
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly int? _currentMainClientId;

    public Repository(DbContext context, int? currentMainClientId = null)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _currentMainClientId = currentMainClientId;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        IQueryable<T> query = _dbSet;
        query = ApplyTenantFilter(query);
        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = _dbSet.Where(predicate);
        query = ApplyTenantFilter(query);
        return await query.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        // Set MainClientID if entity is TenantEntity
        if (entity is TenantEntity tenantEntity && _currentMainClientId.HasValue)
        {
            tenantEntity.MainClientID = _currentMainClientId.Value;
        }

        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        // Set MainClientID for all tenant entities
        if (_currentMainClientId.HasValue)
        {
            foreach (var entity in entities)
            {
                if (entity is TenantEntity tenantEntity)
                {
                    tenantEntity.MainClientID = _currentMainClientId.Value;
                }
            }
        }

        await _dbSet.AddRangeAsync(entities);
        return entities;
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        // Implement soft delete for entities that inherit from BaseEntity
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.DateDeleted = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
        else
        {
            // Hard delete for entities that don't support soft delete
            _dbSet.Remove(entity);
        }
        
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = _dbSet.Where(predicate);
        query = ApplyTenantFilter(query);
        return await query.AnyAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbSet;
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        query = ApplyTenantFilter(query);
        return await query.CountAsync();
    }

    /// <summary>
    /// Apply tenant filter if entity implements TenantEntity
    /// </summary>
    private IQueryable<T> ApplyTenantFilter(IQueryable<T> query)
    {
        if (typeof(TenantEntity).IsAssignableFrom(typeof(T)) && _currentMainClientId.HasValue)
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, nameof(TenantEntity.MainClientID));
            var constant = Expression.Constant(_currentMainClientId.Value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            query = query.Where(lambda);
        }

        return query;
    }
}
