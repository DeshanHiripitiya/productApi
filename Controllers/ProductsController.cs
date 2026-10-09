using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IProductService _service;

    public ProductsController(AppDbContext context, IProductService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products.ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var product = _service.GetById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
public IActionResult Create(Product product)
{
    var createdProduct = _service.Add(product);

    return CreatedAtAction(
        nameof(GetById),
        new { id = createdProduct.Id },
        createdProduct);
}

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Product product)
    {
        return _service.Update(id, product) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return _service.Delete(id) ? NoContent() : NotFound();
    }
}