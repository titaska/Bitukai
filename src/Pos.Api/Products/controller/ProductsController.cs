namespace Pos.Api.Products.controller;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Products.model;
using Pos.Api.Products.service;
using Pos.Api.Products.dto;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductStaffService _productStaffService;

    public ProductsController(IProductService productService,  IProductStaffService productStaffService)
    {
        _productService = productService;
        _productStaffService = productStaffService;
    }

    // GET /products
    [HttpGet]
    public async Task<IActionResult> ListProducts(
        [FromQuery] ProductType? type,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        var (products, total) = await _productService.GetProductsAsync(
            type,
            search,
            page,
            limit
        );

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

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
    {
        var productDto = await _productService.CreateProductAsync(dto);

        return CreatedAtAction(nameof(CreateProduct), new { id = productDto.productId }, productDto);
    }
    
    
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
    
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductUpdateDto dto)
    {
        try
        {
            var product = await _productService.UpdateProductAsync(productId, dto);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        try
        {
            await _productService.DeleteProductAsync(productId);
            return NoContent(); // 204
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{productId}/staff")]
    public async Task<IActionResult> GetEligibleStaff(Guid productId)
    {
        var staff = await _productStaffService.GetEligibleStaffAsync(productId);
        return Ok(staff);
    }
    
    [HttpPost("{productId}/staff")]
    public async Task<IActionResult> LinkStaffToProduct(Guid productId, [FromBody] ProductStaffCreateDto dto)
    {
        var created = await _productStaffService.LinkStaffToProductAsync(productId, dto);
        return CreatedAtAction(nameof(GetEligibleStaff), new { productId }, created);
    }
    
    [HttpPut("{productId}/staff/{staffId}")]
    public async Task<IActionResult> UpdateProductStaff(Guid productId, int staffId, [FromBody] ProductStaffUpdateDto dto)
    {
        var updated = await _productStaffService.UpdateProductStaffAsync(productId, staffId, dto);
        if (updated == null)
            return NotFound($"No ProductStaff link found for productId {productId} and staffId {staffId}");

        return Ok(updated);
    }
    
    [HttpDelete("{productId}/staff/{staffId}")]
    public async Task<IActionResult> UnlinkStaffFromProduct(Guid productId, int staffId)
    {
        var success = await _productStaffService.UnlinkStaffFromProductAsync(productId, staffId);
        if (!success)
            return NotFound($"No ProductStaff link found for productId {productId} and staffId {staffId}");

        return NoContent();
    }
}
