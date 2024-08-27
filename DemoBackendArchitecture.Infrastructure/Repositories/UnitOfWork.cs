using System.Linq.Expressions;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

[RegisterAsScoped]
public class UnitOfWork<T>(ApplicationDbContext context) : IUnitOfWork<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(T entity, CancellationToken token) => await _dbSet.AddAsync(entity, token);

    public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

    #region Read
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter) => await _dbSet.Where(filter).ToListAsync();
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter) => await _dbSet.AnyAsync(filter);

    public async Task<bool> AnyAsync() => await _dbSet.AnyAsync();

    public async Task<int> CountAsync(Expression<Func<T, bool>> filter) => await _dbSet.CountAsync(filter);

    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    public async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        => await _dbSet.IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefaultAsync(filter);

    #endregion

    #region Update and Delete

    public void Update(T entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public async Task Delete(object id)
    {
       T entity = await GetByIdAsync(id);
         Delete(entity);
    }
/// <summary>
/// 
/// </summary>
/// <returns></returns>
    public int SaveChange() => _context.SaveChanges();

    public async Task SaveChangeAsync(CancellationToken token) => await _context.SaveChangesAsync(token);

    #endregion
    
}