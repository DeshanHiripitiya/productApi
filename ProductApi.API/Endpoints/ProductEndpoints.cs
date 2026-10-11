using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProductApi.API.Contracts;
using ProductApi.Application;

namespace ProductApi.API.Endpoints;

public static class ProductEndpoints
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/products", async Task<Ok<IReadOnlyList<ProductResponse>>> (
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var products = await productService.GetAllAsync(cancellationToken);
            var response = products
                .Select(product => new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Price))
                .ToArray();

            return TypedResults.Ok<IReadOnlyList<ProductResponse>>(response);
        })
        .WithName("GetProducts")
        .WithSummary("List products")
        .WithDescription("Returns all products.")
        .Produces<IReadOnlyList<ProductResponse>>(StatusCodes.Status200OK);

        app.MapGet("/api/products/{id:int}", async Task<Results<
            Ok<ProductResponse>,
            NotFound,
            ProblemHttpResult>> (
            int id,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            if (id <= 0)
            {
                return TypedResults.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid product ID",
                    detail: "Product ID must be greater than zero.");
            }

            var product = await productService.GetByIdAsync(
                id,
                cancellationToken);

            return product is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Price));
        })
        .WithName("GetProductById")
        .WithSummary("Get a product by ID")
        .WithDescription("Returns one product or 404 if it does not exist.")
        .Produces<ProductResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/api/products", async Task<Results<
            Created<ProductResponse>,
            ValidationProblem>> (
            CreateProductRequest request,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                request,
                new ValidationContext(request),
                validationResults,
                validateAllProperties: true);

            if (!isValid)
            {
                var errors = validationResults
                    .SelectMany(result => result.MemberNames.Select(memberName => new
                    {
                        MemberName = memberName,
                        ErrorMessage = result.ErrorMessage ?? "The value is invalid."
                    }))
                    .GroupBy(error => error.MemberName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray());

                return TypedResults.ValidationProblem(errors);
            }

            var product = await productService.AddAsync(
                request.Name,
                request.Price,
                cancellationToken);
            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Price);

            return TypedResults.Created($"/api/products/{product.Id}", response);
        })
        .WithName("CreateProduct")
        .WithSummary("Create a product")
        .WithDescription("Creates a product and returns its resource URL.")
        .Produces<ProductResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        return app;
    }
}
