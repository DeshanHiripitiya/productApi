namespace ProductApi.Application;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ProductDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ProductDto> AddAsync(
        string name,
        decimal price,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
    int id,
    CancellationToken cancellationToken = default);
}
