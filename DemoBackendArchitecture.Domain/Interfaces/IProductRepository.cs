using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    public IEnumerable<Product> GetByName(string name);
    
    //Demo it thui nhe
    IQueryable<Product> Filter(IQueryable<Product> queryable, string name = "", decimal minPrice = 0,
        decimal maxPrice = decimal.MaxValue, int minStock = 0, int maxStock = int.MaxValue, string description = "");
    
    //Sort product by categories
    public IQueryable<Product> Sort(IQueryable<Product> queryable, string sortOrder);
}