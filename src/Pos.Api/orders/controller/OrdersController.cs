using Microsoft.AspNetCore.Mvc;
using Pos.Api.Context;
using Pos.Api.orders.dto;
using Pos.Api.Orders.Model;
using Pos.Api.orders.service;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderLineService _orderLineService;

    public OrdersController(IOrderService orderService,  IOrderLineService orderLineService)
    {
        _orderService = orderService;
        _orderLineService = orderLineService;
    }

    // GET /orders
    [HttpGet]
    public async Task<IActionResult> ListOrders(
        [FromQuery] string? status,
        [FromQuery] Guid? customerId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        OrderStatus? orderStatus = null;

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            orderStatus = parsedStatus;

        var (orders, total) = await _orderService.GetOrdersAsync(orderStatus, customerId, fromDate, toDate, page, limit);

        var response = new
        {
            data = orders,
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

    // POST /orders
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.registrationNumber))
            return BadRequest("RegistrationNumber is required.");

        var orderDto = await _orderService.CreateOrderAsync(dto);

        return CreatedAtAction(nameof(CreateOrder), new { id = orderDto.orderId }, orderDto);
    }

    // GET /orders/{orderId}
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    // PUT /orders/{orderId}
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderUpdateDto dto)
    {
        try
        {
            var order = await _orderService.UpdateOrderAsync(orderId, dto);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST /orders/{orderId}/lines
    [HttpPost("{orderId}/lines")]
    public async Task<IActionResult> AddLine(Guid orderId, [FromBody] OrderLineCreateDto dto)
    {
        try
        {
            var line = await _orderLineService.AddOrderLineAsync(orderId, dto);
            return CreatedAtAction(nameof(GetOrder), new { orderId = orderId }, line);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT /orders/{orderId}/lines/{lineId}
    [HttpPut("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> UpdateLine(Guid orderId, Guid lineId, [FromBody] OrderLineUpdateDto dto)
    {
        try
        {
            var updatedLine = await _orderLineService.UpdateLineAsync(orderId, lineId, dto);

            var result = new OrderLineDto
            {
                orderLineId = updatedLine.orderLineId,
                orderId = updatedLine.orderId,
                productId = updatedLine.productId,
                quantity = updatedLine.quantity,
                assignedStaffId = updatedLine.assignedStaffId,
                appointmentId = updatedLine.appointmentId,
                notes = updatedLine.notes,
                unitPrice = updatedLine.unitPrice,
                subTotal = updatedLine.subTotal,
                //options = new List<OrderLineOptionDto>()
            };

            return Ok(result);
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

    // DELETE /orders/{orderId}/lines/{lineId}
    [HttpDelete("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> DeleteLine(Guid orderId, Guid lineId)
    {
        try
        {
            await _orderLineService.DeleteLineAsync(orderId, lineId);
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

    // POST /orders/{orderId}/close
    [HttpPost("{orderId}/close")]
    public async Task<IActionResult> CloseOrder(string orderId)
    {
        if (!Guid.TryParse(orderId, out var orderGuid))
            return BadRequest("Invalid orderId");

        var closedOrder = await _orderService.CloseOrderAsync(orderGuid);

        return Ok(closedOrder);
    }
}
