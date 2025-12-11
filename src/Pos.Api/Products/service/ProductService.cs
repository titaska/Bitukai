namespace Pos.Api.Products.service;
using Pos.Api.Context;
using Pos.Api.Products.dto;
using Pos.Api.Products.model;
using Microsoft.EntityFrameworkCore;

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

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Product> products, int total)> GetProductsAsync(ProductType? type, string? search, int page, int limit)
    {
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

        return (products, total);
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.productId == productId);
    }

    public async Task<Product> CreateProductAsync(ProductCreateDto dto)
    {
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

        return product;
    }

    public async Task<Product> UpdateProductAsync(Guid productId, ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return null;

        // Since all fields are required in DTO (except durationMinutes), we can overwrite directly
        product.name = dto.name;
        product.description = dto.description;
        product.basePrice = dto.basePrice;
        product.durationMinutes = dto.durationMinutes;
        product.taxCode = dto.taxCode;
        product.status = dto.status;

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new KeyNotFoundException("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
