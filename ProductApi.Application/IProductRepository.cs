
using ProductApi.Domain;

namespace ProductApi.Application;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default);
}
