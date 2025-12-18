using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pos.Api.orders.dto;
using Pos.Api.Orders.Model;
using Pos.Api.orders.service;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderLineService _orderLineService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        IOrderLineService orderLineService,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _orderLineService = orderLineService;
        _logger = logger;
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
        _logger.LogInformation(
            "Listing orders: Status={Status}, CustomerId={CustomerId}, From={FromDate}, To={ToDate}, Page={Page}, Limit={Limit}",
            status,
            customerId,
            fromDate,
            toDate,
            page,
            limit
        );

        OrderStatus? orderStatus = null;

        if (!string.IsNullOrEmpty(status) &&
            Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
        {
            orderStatus = parsedStatus;
        }

        var (orders, total) = await _orderService.GetOrdersAsync(
            orderStatus,
            customerId,
            fromDate,
            toDate,
            page,
            limit
        );

        _logger.LogInformation(
            "Retrieved {Count} orders (Total={Total})",
            orders.Count(),
            total
        );

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
        {
            _logger.LogWarning("CreateOrder failed: RegistrationNumber missing");
            return BadRequest("RegistrationNumber is required.");
        }

        _logger.LogInformation(
            "Creating order for registration {RegistrationNumber}",
            dto.registrationNumber
        );

        var orderDto = await _orderService.CreateOrderAsync(dto);

        _logger.LogInformation(
            "Order created successfully: OrderId={OrderId}",
            orderDto.orderId
        );

        return CreatedAtAction(nameof(CreateOrder), new { id = orderDto.orderId }, orderDto);
    }

    // GET /orders/{orderId}
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        _logger.LogInformation("Fetching order {OrderId}", orderId);

        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return NotFound();
        }

        return Ok(order);
    }

    // PUT /orders/{orderId}
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderUpdateDto dto)
    {
        _logger.LogInformation("Updating order {OrderId}", orderId);

        try
        {
            var order = await _orderService.UpdateOrderAsync(orderId, dto);

            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found for update", orderId);
                return NotFound();
            }

            _logger.LogInformation("Order {OrderId} updated successfully", orderId);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid update attempt for order {OrderId}",
                orderId
            );

            return BadRequest(ex.Message);
        }
    }

    // POST /orders/{orderId}/lines
    [HttpPost("{orderId}/lines")]
    public async Task<IActionResult> AddLine(Guid orderId, [FromBody] OrderLineCreateDto dto)
    {
        _logger.LogInformation("Adding order line to order {OrderId}", orderId);

        try
        {
            var line = await _orderLineService.AddOrderLineAsync(orderId, dto);

            _logger.LogInformation(
                "Order line added: LineId={LineId} OrderId={OrderId}",
                line.orderLineId,
                orderId
            );

            return CreatedAtAction(nameof(GetOrder), new { orderId }, line);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Order {OrderId} not found when adding line",
                orderId
            );

            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid attempt to add line to order {OrderId}",
                orderId
            );

            return BadRequest(ex.Message);
        }
    }

    // PUT /orders/{orderId}/lines/{lineId}
    [HttpPut("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> UpdateLine(
        Guid orderId,
        Guid lineId,
        [FromBody] OrderLineUpdateDto dto)
    {
        _logger.LogInformation(
            "Updating order line {LineId} for order {OrderId}",
            lineId,
            orderId
        );

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
                subTotal = updatedLine.subTotal
            };

            _logger.LogInformation(
                "Order line {LineId} updated successfully",
                lineId
            );

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Order or line not found: OrderId={OrderId}, LineId={LineId}",
                orderId,
                lineId
            );

            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid update attempt for line {LineId}",
                lineId
            );

            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /orders/{orderId}/lines/{lineId}
    [HttpDelete("{orderId}/lines/{lineId}")]
    public async Task<IActionResult> DeleteLine(Guid orderId, Guid lineId)
    {
        _logger.LogInformation(
            "Deleting order line {LineId} from order {OrderId}",
            lineId,
            orderId
        );

        try
        {
            await _orderLineService.DeleteLineAsync(orderId, lineId);

            _logger.LogInformation(
                "Order line {LineId} deleted successfully",
                lineId
            );

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Order or line not found during delete: OrderId={OrderId}, LineId={LineId}",
                orderId,
                lineId
            );

            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid delete attempt for line {LineId}",
                lineId
            );

            return BadRequest(new { message = ex.Message });
        }
    }

    // POST /orders/{orderId}/close
    [HttpPost("{orderId}/close")]
    public async Task<IActionResult> CloseOrder(string orderId)
    {
        if (!Guid.TryParse(orderId, out var orderGuid))
        {
            _logger.LogWarning("CloseOrder called with invalid orderId: {OrderId}", orderId);
            return BadRequest("Invalid orderId");
        }

        _logger.LogInformation("Closing order {OrderId}", orderGuid);

        var closedOrder = await _orderService.CloseOrderAsync(orderGuid);

        _logger.LogInformation("Order {OrderId} closed successfully", orderGuid);

        return Ok(closedOrder);
    }

    // POST /orders/{orderId}/calculate
    [HttpPost("{orderId}/calculate")]
    public async Task<IActionResult> CalculateOrder(string orderId)
    {
        if (!Guid.TryParse(orderId, out var orderGuid))
        {
            _logger.LogWarning("CalculateOrder called with invalid orderId: {OrderId}", orderId);
            return BadRequest("Invalid orderId");
        }

        _logger.LogInformation("Calculating order {OrderId}", orderGuid);

        var calculatedOrder = await _orderService.CalculateOrderAsync(orderGuid);

        _logger.LogInformation("Order {OrderId} calculated successfully", orderGuid);

        return Ok(calculatedOrder);
    }
}
