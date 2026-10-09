using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : IProductService
{

    private int _nextId = 3;

    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 250000 },
        new Product { Id = 2, Name = "Mouse", Price = 5000 }
    };

    public List<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public Product Add(Product product)
{
    product.Id = _nextId++;
    _products.Add(product);

    return product;
}

    public bool Update(int id, Product product)
    {
        var existingProduct = GetById(id);
        if (existingProduct is null)
        {
            return false;
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        return true;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        return product is not null && _products.Remove(product);
    }
}