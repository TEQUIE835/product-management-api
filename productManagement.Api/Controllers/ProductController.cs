using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using productManagement.Application.Interfaces.Auth;
using productManagement.Application.Interfaces.Products;
using productManagement.Domain.Entities;

namespace productManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService  _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Obtiene la lista completa de productos registrados.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Obtiene los detalles de un producto específico por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// Crea un nuevo producto en el sistema (solo rol Admin).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            await _productService.AddProduct(product);
            return Ok("product created");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// Actualiza la información de un producto existente.
    /// </summary>

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            await _productService.UpdateProduct(id, product);
            return Ok("product updated");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    /// <summary>
    /// Elimina un producto del sistema (solo rol Admin).
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return Ok("product deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}