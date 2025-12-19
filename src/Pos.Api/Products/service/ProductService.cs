namespace Pos.Api.Products.service;
using Pos.Api.Context;
using Pos.Api.Products.dto;
using Pos.Api.Products.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IProductService
{
    Task<(IEnumerable<Product> products, int total)> GetProductsAsync(ProductType? type, string? search, int page, int limit);
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<Product> CreateProductAsync(ProductCreateDto dto);
    Task<Product> UpdateProductAsync(Guid productId, ProductUpdateDto dto);
    Task DeleteProductAsync(Guid productId);
}

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(IEnumerable<Product> products, int total)> GetProductsAsync(ProductType? type, string? search, int page, int limit)
    {
        _logger.LogInformation("Fetching products. Type={Type}, Search={Search}, Page={Page}, Limit={Limit}", type, search, page, limit);

        var query = _context.Products.AsQueryable();

        if (type.HasValue)
            query = query.Where(p => p.type == type.Value);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.name.Contains(search) || p.description.Contains(search));

        var total = await query.CountAsync();
        var products = await query
            .OrderBy(p => p.name)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        _logger.LogInformation("Fetched {Count} products (total {Total})", products.Count, total);
        return (products, total);
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        _logger.LogInformation("Fetching product by ID: {ProductId}", productId);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.productId == productId);
        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found", productId);
        }
        else
        {
            _logger.LogInformation("Fetched product {ProductId}: Name={Name}", productId, product.name);
        }

        return product;
    }

    public async Task<Product> CreateProductAsync(ProductCreateDto dto)
    {
        _logger.LogInformation("Creating product: Name={Name}, Type={Type}", dto.name, dto.productType);

        var product = new Product
        {
            productId = Guid.NewGuid(),
            registrationNumber = dto.registrationNumber,
            type = dto.productType,
            name = dto.name,
            description = dto.description,
            basePrice = dto.basePrice,
            taxCode = dto.taxCode,
            status = dto.status,
            durationMinutes = dto.durationMinutes
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created product {ProductId}: Name={Name}", product.productId, product.name);
        return product;
    }

    public async Task<Product> UpdateProductAsync(Guid productId, ProductUpdateDto dto)
    {
        _logger.LogInformation("Updating product {ProductId}", productId);

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found for update", productId);
            return null;
        }

        product.name = dto.name;
        product.description = dto.description;
        product.basePrice = dto.basePrice;
        product.durationMinutes = dto.durationMinutes;
        product.taxCode = dto.taxCode;
        product.status = dto.status;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated product {ProductId}: Name={Name}", productId, product.name);

        return product;
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        _logger.LogInformation("Deleting product {ProductId}", productId);

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found for deletion", productId);
            throw new KeyNotFoundException("Product not found");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted product {ProductId}: Name={Name}", productId, product.name);
    }
}
