using Moq;
using ProductApi.Application;
using ProductApi.Domain;

namespace ProductApi.Application.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task AddAsync_WithValidProduct_TrimsNameAndReturnsSavedProduct()
    {
        // Arrange
        Product? savedProduct = null;
        var repository = new Mock<IProductRepository>();

        repository
            .Setup(repository => repository.AddAsync(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((product, _) =>
            {
                product.Id = 7; // Simulate the database-generated ID
                savedProduct = product;
            })
            .Returns(Task.CompletedTask);

        var service = new ProductService(repository.Object);

        // Act
        var result = await service.AddAsync(" Wireless Mouse ", 29.99m);

        // Assert
        Assert.NotNull(savedProduct);
        Assert.Equal(7, result.Id);
        Assert.Equal("Wireless Mouse", result.Name);
        Assert.Equal(29.99m, result.Price);
        Assert.Equal(result.Name, savedProduct.Name);

        repository.Verify(
            repository => repository.AddAsync(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}