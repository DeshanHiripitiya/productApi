
using Microsoft.EntityFrameworkCore;
using ProductApi.Application;
using ProductApi.Domain;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(
    int id,
    CancellationToken cancellationToken = default)
{
    var product = await _context.Products
        .FirstOrDefaultAsync(
            product => product.Id == id,
            cancellationToken);

    if (product is null)
    {
        return false;
    }

    _context.Products.Remove(product);
    await _context.SaveChangesAsync(cancellationToken);

    return true;
}
}
