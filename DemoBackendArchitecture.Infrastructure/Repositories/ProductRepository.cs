using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : GenericRepository<Product>(context),IProductRepository
{
    //inherit Interface of domain layer
    public IEnumerable<Product> GetByName(string name)
    {
        return context.Products.Where(p => p.Name!=null && p.Name.Contains(name));
    }

    public IQueryable<Product> Filter(IQueryable<Product> queryable, string name = "", decimal minPrice = 0, decimal maxPrice = Decimal.MaxValue,
        int minStock = 0, int maxStock = Int32.MaxValue, string description = "")
    {
        //Filter by name
        if (!string.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(p => p.Name!=null && p.Name.Contains(name));
        }
        
        //Filer price by minPrice and maxPrice
        queryable = queryable.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        
        //Filter stock by minStock and maxStock
        queryable = queryable.Where(p => p.Stock >= minStock && p.Stock <= maxStock);
        
        //Filter by description
        if (!string.IsNullOrEmpty(description))
        {
            queryable = queryable.Where(p => p.Description!= null && p.Description.Contains(description));
        }

        return queryable;
    }
    
    public IQueryable<Product> Sort(IQueryable<Product> queryable, string sortOrder)
    {
        //Sort by name
        queryable = sortOrder switch
        {
            "name_desc" => queryable.OrderByDescending(p => p.Name),
            _ => queryable.OrderBy(p => p.Name)
        };

        return queryable;
    }
}