using System.Linq.Expressions;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IUnitOfWork<T> where T : class
{
    /// <summary>
    /// Add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task AddAsync(T entity, CancellationToken token);
    /// <summary>
    /// Add range of entities to database
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public Task AddRangeAsync(IEnumerable<T> entities);
    /// <summary>
    /// Get all entities from database
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetAllAsync();
    /// <summary>
    /// Get all entities from database with filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    /// <summary>
    /// Check if any entity exists in database with filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    /// <summary>
    /// Check if any entity exists in database
    /// </summary>
    /// <returns></returns>
    public Task<bool> AnyAsync();
    public Task<int> CountAsync(Expression<Func<T, bool>> filter);
    /// <summary>
    /// Count all entities in database
    /// </summary>
    /// <returns></returns>
    public Task<int> CountAsync();
    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<T> GetByIdAsync(object id);
    /// <summary>
    /// Get first entity or default with filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    /// <summary>
    /// Update entity in database
    /// </summary>
    /// <param name="entity"></param>
    public void Update(T entity);
    /// <summary>
    /// Update range of entities in database
    /// </summary>
    /// <param name="entities"></param>
    public void UpdateRange(IEnumerable<T> entities);
    /// <summary>
    /// Delete entity from database
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(T entity);
    /// <summary>
    /// Delete range of entities from database
    /// </summary>
    /// <param name="entities"></param>
    public void DeleteRange(IEnumerable<T> entities);
    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task Delete(object id);
    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <returns></returns>
    public int SaveChange();
    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task SaveChangeAsync(CancellationToken token);
    
}