using productManagement.Application.Services.Products;

namespace productManagement.Tests.Tests;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _productService = new ProductService(_productRepoMock.Object);
    }

    [Fact]
    public async Task AddProduct_ShouldThrow_WhenExists()
    {
        _productRepoMock.Setup(r => r.GetByNameAsync("phone"))
            .ReturnsAsync(new Product { Id = 1, Name = "phone" });

        var product = new Product { Name = "phone" };

        Func<Task> act = async () => await _productService.AddProduct(product);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Product name already exists");
    }

    [Fact]
    public async Task AddProduct_ShouldAdd_WhenValid()
    {
        _productRepoMock.Setup(r => r.GetByNameAsync("phone")).ReturnsAsync((Product?)null);

        var product = new Product { Name = "phone" };

        await _productService.AddProduct(product);

        _productRepoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == "phone")), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrow_WhenNotFound()
    {
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        Func<Task> act = async () => await _productService.UpdateProduct(1, new Product());

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Product not found");
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdate_WhenValid()
    {
        var existing = new Product { Id = 1, Name = "old" };
        _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        var updated = new Product { Name = "new", Description = "desc", Price = 100 };

        await _productService.UpdateProduct(1, updated);

        _productRepoMock.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Name == "new" && p.Price == 100)), Times.Once);
    }
}