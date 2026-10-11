using System.ComponentModel.DataAnnotations;

namespace ProductApi.API.Contracts;

/// <summary>Request payload for creating a product.</summary>
public sealed record CreateProductRequest
{
    /// <summary>The product name.</summary>
    [Required, StringLength(200, MinimumLength = 1)]
    public required string Name { get; init; }

    /// <summary>The product price, which must be greater than zero.</summary>
    [Range(0.01, 999999999.99)]
    public required decimal Price { get; init; }
}

/// <summary>Product data returned by the API.</summary>
public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price);
