using System.Linq.Expressions;
using DemoBackendArchitecture.Application.Helpers;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    #region Add, Get, Pagination

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

    public async Task<bool> AnyAsync() => await _dbSet.AnyAsync();

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate)
    {
        return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
    }

    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    public async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task<Pagination<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageIndex = 1, int pageSize = 10)
    {
        var itemCount = await _dbSet.CountAsync();
        var item = await _dbSet.Where(predicate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var result = new Pagination<T>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = item
        };

        return result;
    }

    public async Task<Pagination<T>> ToPagination(int pageIndex = 1, int pageSize = 10)
    {
        var itemCount = await _dbSet.CountAsync();
        var item = await _dbSet
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        
        var result = new Pagination<T>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = item
        };
        return result;
    }

    #endregion

    #region Update and Delete
    
    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

    public void Update(T entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public async Task DeleteById(object id)
    {
        var entity = await GetByIdAsync(id);
        Delete(entity);
    }

    #endregion
}