using productManagement.Domain.Entities;

namespace productManagement.Application.Interfaces.Products;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product> GetProductById(int id);
    Task AddProduct(Product product);
    Task UpdateProduct(int id, Product product);
    Task DeleteProduct(int id);

}