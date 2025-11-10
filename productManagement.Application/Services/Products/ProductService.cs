using productManagement.Application.Interfaces.Products;
using productManagement.Domain.Entities;
using productManagement.Domain.Interfaces;

namespace productManagement.Application.Services.Products;

public class ProductService :IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _productRepository.GetAllAsync();
    }
    
    public async Task<Product> GetProductById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) throw new ArgumentException("Product not found");
        return product;
    }

    public async Task AddProduct(Product product)
    {
        var exists = await _productRepository.GetByNameAsync(product.Name);
        if (exists != null) throw new InvalidOperationException("Product name already exists");
        await _productRepository.AddAsync(product);
    }
    public async Task UpdateProduct(int id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) throw new ArgumentException("Product not found");
        if (existingProduct.Name == product.Name && existingProduct.Id != id) throw new InvalidOperationException("Product name already exists");
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        await _productRepository.UpdateAsync(existingProduct);
    }
    
    public async Task DeleteProduct(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) throw new ArgumentException("Product not found");
        await _productRepository.DeleteAsync(existingProduct);
    }
}