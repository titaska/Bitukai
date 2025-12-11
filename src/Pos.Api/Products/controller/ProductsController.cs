namespace Pos.Api.Products.controller;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Products.model;
using Pos.Api.Products.service;
using Pos.Api.Products.dto;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
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
    
}
