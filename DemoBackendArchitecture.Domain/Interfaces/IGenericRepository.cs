using System.Linq.Expressions;
using DemoBackendArchitecture.Application.Helpers;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IGenericRepository<T> where T:class
{
    public Task AddAsync(T entity);
    public Task AddRangeAsync(IEnumerable<T> entities);
    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    public Task<bool> AnyAsync();
    public Task<int> CountAsync(Expression<Func<T, bool>>? predicate);
    public Task<int> CountAsync();
    public Task<T> GetByIdAsync(object id);
    public Task<Pagination<T>> GetAsync(Expression<Func<T, bool>> predicate, int pageIndex = 1, int pageSize = 10);
    public Task<Pagination<T>> ToPagination(int pageIndex = 1, int pageSize = 10);
    
    public Task<T> FirstOrDefaultAsync (Expression<Func<T, bool>> predicate);
    public void Update(T entity);
    public void UpdateRange (IEnumerable<T> entities);
    public void Delete(T entity);
    public void DeleteRange(IEnumerable<T> entities);
    public Task DeleteById(object id);
}