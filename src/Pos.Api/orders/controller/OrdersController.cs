using Microsoft.AspNetCore.Mvc;
using Pos.Api.Context;
using Pos.Api.orders.dto;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // GET /orders
    [HttpGet]
    public async Task<IActionResult> ListOrders(
        [FromQuery] string? status,
        [FromQuery] string? customerId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 50)
    {
        return Ok();
    }

    // POST /orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreate dto)
    {
        return Ok();
    }

    // GET /orders/{orderId}
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(string orderId)
    {
        return Ok();
    }

    // PUT /orders/{orderId}
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderUpdate dto)
    {
        return Ok();
    }

    // POST /orders/{orderId}/lines
    [HttpPost("{orderId}/lines")]
    public async Task<IActionResult> AddLine(string orderId, [FromBody] OrderLineCreate dto)
    {
        return Ok();
    }

    // PUT /orders/{orderId}/lines/{lineId}
    [HttpPut("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> UpdateLine(string orderId, string lineId, [FromBody] OrderLineUpdate dto)
    {
        return Ok();
    }

    // DELETE /orders/{orderId}/lines/{lineId}
    [HttpDelete("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> DeleteLine(string orderId, string lineId)
    {
        return NoContent();
    }

    // PUT /orders/{orderId}/tip
    [HttpPut("{orderId}/tip")]
    public async Task<IActionResult> UpdateTip(string orderId, [FromBody] TipUpdate dto)
    {
        return Ok();
    }

    // POST /orders/{orderId}/discounts
    [HttpPost("{orderId}/discounts")]
    public async Task<IActionResult> ApplyDiscount(string orderId, [FromBody] OrderDiscountCreate dto)
    {
        return Ok();
    }

    // POST /orders/{orderId}/close
    [HttpPost("{orderId}/close")]
    public async Task<IActionResult> CloseOrder(string orderId)
    {
        return Ok();
    }
}
