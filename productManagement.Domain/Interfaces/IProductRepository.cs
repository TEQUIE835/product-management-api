using productManagement.Domain.Entities;

namespace productManagement.Domain.Interfaces;

public interface IProductRepository
{
    public Task AddAsync(Product product);
    public Task<List<Product>> GetAllAsync();
    public Task<Product?> GetByIdAsync(int id);
    public Task<Product?> GetByNameAsync(string name);
    public Task UpdateAsync(Product product);
    public Task DeleteAsync(Product product);
}