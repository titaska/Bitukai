namespace Pos.Api.Products.controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.Products.model;
using Pos.Api.Products.service;
using Pos.Api.Products.dto;
using System.Linq;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductStaffService _productStaffService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, IProductStaffService productStaffService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _productStaffService = productStaffService;
        _logger = logger;
    }

    // GET /products
    [HttpGet]
    public async Task<IActionResult> ListProducts(
        [FromQuery] ProductType? type,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        _logger.LogInformation("Listing products. Type={Type}, Search={Search}, Page={Page}, Limit={Limit}", type, search, page, limit);

        var (products, total) = await _productService.GetProductsAsync(type, search, page, limit);

        var response = new
        {
            data = products,
            pagination = new
            {
                page,
                limit,
                total,
                totalPages = (int)Math.Ceiling((double)total / limit)
            }
        };

        _logger.LogInformation("Listed {Count} products", products.Count());
        return Ok(response);
    }

    // POST /products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
    {
        _logger.LogInformation("Creating product: Name={Name}, Type={Type}", dto.name, dto.productType);

        var productDto = await _productService.CreateProductAsync(dto);

        _logger.LogInformation("Created product {ProductId}", productDto.productId);
        return CreatedAtAction(nameof(CreateProduct), new { id = productDto.productId }, productDto);
    }

    // GET /products/{productId}
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        _logger.LogInformation("Fetching product {ProductId}", productId);

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found", productId);
            return NotFound();
        }

        _logger.LogInformation("Fetched product {ProductId}", productId);
        return Ok(product);
    }

    // PUT /products/{productId}
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductUpdateDto dto)
    {
        _logger.LogInformation("Updating product {ProductId}", productId);

        try
        {
            var product = await _productService.UpdateProductAsync(productId, dto);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found for update", productId);
                return NotFound();
            }

            _logger.LogInformation("Updated product {ProductId}", productId);
            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to update product {ProductId}", productId);
            return BadRequest(ex.Message);
        }
    }

    // DELETE /products/{productId}
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        _logger.LogInformation("Deleting product {ProductId}", productId);

        try
        {
            await _productService.DeleteProductAsync(productId);
            _logger.LogInformation("Deleted product {ProductId}", productId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Product {ProductId} not found for deletion", productId);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to delete product {ProductId}", productId);
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /products/{productId}/staff
    [HttpGet("{productId}/staff")]
    public async Task<IActionResult> GetEligibleStaff(Guid productId)
    {
        _logger.LogInformation("Fetching eligible staff for product {ProductId}", productId);
        var staff = await _productStaffService.GetEligibleStaffAsync(productId);
        _logger.LogInformation("Found {Count} staff members for product {ProductId}", staff.Count(), productId);
        return Ok(staff);
    }

    // POST /products/{productId}/staff
    [HttpPost("{productId}/staff")]
    public async Task<IActionResult> LinkStaffToProduct(Guid productId, [FromBody] ProductStaffCreateDto dto)
    {
        _logger.LogInformation("Linking staff {StaffId} to product {ProductId}", dto.staffId, productId);
        var created = await _productStaffService.LinkStaffToProductAsync(productId, dto);
        _logger.LogInformation("Linked staff {StaffId} to product {ProductId}", dto.staffId, productId);
        return CreatedAtAction(nameof(GetEligibleStaff), new { productId }, created);
    }

    // PUT /products/{productId}/staff/{staffId}
    [HttpPut("{productId}/staff/{staffId}")]
    public async Task<IActionResult> UpdateProductStaff(Guid productId, Guid staffId, [FromBody] ProductStaffUpdateDto dto)
    {
        _logger.LogInformation("Updating ProductStaff link: ProductId={ProductId}, StaffId={StaffId}", productId, staffId);
        var updated = await _productStaffService.UpdateProductStaffAsync(productId, staffId, dto);
        if (updated == null)
        {
            _logger.LogWarning("No ProductStaff link found for ProductId={ProductId} StaffId={StaffId}", productId, staffId);
            return NotFound($"No ProductStaff link found for productId {productId} and staffId {staffId}");
        }

        _logger.LogInformation("Updated ProductStaff link: ProductId={ProductId}, StaffId={StaffId}", productId, staffId);
        return Ok(updated);
    }

    // DELETE /products/{productId}/staff/{staffId}
    [HttpDelete("{productId}/staff/{staffId}")]
    public async Task<IActionResult> UnlinkStaffFromProduct(Guid productId, Guid staffId)
    {
        _logger.LogInformation("Unlinking staff {StaffId} from product {ProductId}", staffId, productId);
        var success = await _productStaffService.UnlinkStaffFromProductAsync(productId, staffId);

        if (!success)
        {
            _logger.LogWarning("No ProductStaff link found for ProductId={ProductId} StaffId={StaffId}", productId, staffId);
            return NotFound($"No ProductStaff link found for productId {productId} and staffId {staffId}");
        }

        _logger.LogInformation("Unlinked staff {StaffId} from product {ProductId}", staffId, productId);
        return NoContent();
    }
}
