using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pos.Api.Context;
using Pos.Api.orders.dto;
using Pos.Api.Orders.Model;
using Pos.Api.orders.service;

namespace Pos.Api.orders.service;

public interface IOrderService
{
    Task<(List<OrderDto> Orders, int Total)> GetOrdersAsync(
        OrderStatus? status = null,
        Guid? customerId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int page = 1,
        int limit = 20
    );

    Task<OrderDto> CreateOrderAsync(OrderCreateDto input);

    Task<OrderDto?> GetOrderByIdAsync(Guid orderId);

    Task<OrderDto?> UpdateOrderAsync(Guid orderId, OrderUpdateDto input);

    Task<OrderDto> CloseOrderAsync(Guid orderId);
    Task<OrderDto> CalculateOrderAsync(Guid orderId);
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IOrderLineService _orderLineService;
    private readonly ILogger<OrderService> _logger;
    
    public OrderService(AppDbContext dbContext, IOrderLineService orderLineService, ILogger<OrderService> logger)
    {
        _context = dbContext;
        _orderLineService = orderLineService;
        _logger = logger;
    }

    public async Task<(List<OrderDto> Orders, int Total)> GetOrdersAsync(
        OrderStatus? status = null,
        Guid? customerId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int page = 1,
        int limit = 20
    )
    {
        _logger.LogInformation("Fetching orders: Status={Status}, CustomerId={CustomerId}, From={From}, To={To}, Page={Page}, Limit={Limit}",
            status, customerId, fromDate, toDate, page, limit);

        var query = _context.Orders.Include(o => o.Lines).AsQueryable();

        if (status.HasValue) query = query.Where(o => o.status == status.Value);
        if (customerId.HasValue) query = query.Where(o => o.customerId == customerId.Value);
        if (fromDate.HasValue) query = query.Where(o => o.createdAt >= fromDate.Value);
        if (toDate.HasValue) query = query.Where(o => o.createdAt <= toDate.Value);

        int total = await query.CountAsync();
        var orders = await query.OrderByDescending(o => o.createdAt)
                                .Skip((page - 1) * limit)
                                .Take(limit)
                                .ToListAsync();

        _logger.LogInformation("Retrieved {Count} orders (Total={Total})", orders.Count, total);

        var orderDtos = orders.Select(o => new OrderDto
        {
            orderId = o.orderId,
            registrationNumber = o.registrationNumber,
            customerId = o.customerId,
            status = o.status,
            createdAt = o.createdAt,
            closedAt = o.closedAt,
            serviceChargePct = o.serviceChargePct,
            tipAmount = o.tipAmount,
            subtotalAmount = o.subtotalAmount,
            taxAmount = o.taxAmount,
            discountAmount = o.discountAmount,
            serviceChargeAmount = o.serviceChargeAmount,
            totalDue = o.totalDue,
            lines = o.Lines.Select(l => new OrderLineDto
            {
                orderLineId = l.orderLineId,
                orderId = l.orderId,
                productId = l.productId,
                quantity = l.quantity,
                assignedStaffId = l.assignedStaffId,
                appointmentId = l.appointmentId,
                notes = l.notes,
                unitPrice = l.unitPrice,
                subTotal = l.subTotal
            }).ToList(),
        }).ToList();

        return (orderDtos, total);
    }

    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto input)
    {
        _logger.LogInformation("Creating new order for registration {RegistrationNumber}", input.registrationNumber);

        var order = new Order
        {
            orderId = Guid.NewGuid(),
            registrationNumber = input.registrationNumber,
            customerId = input.customerId,
            status = OrderStatus.OPEN,
            createdAt = DateTime.UtcNow,
            closedAt = null,
            serviceChargePct = 0,
            tipAmount = 0,
            subtotalAmount = 0,
            taxAmount = 0,
            discountAmount = 0,
            serviceChargeAmount = 0,
            totalDue = 0
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order created: OrderId={OrderId}", order.orderId);

        return new OrderDto
        {
            orderId = order.orderId,
            registrationNumber = order.registrationNumber,
            customerId = order.customerId,
            status = order.status,
            createdAt = order.createdAt,
            closedAt = order.closedAt,
            serviceChargePct = order.serviceChargePct,
            tipAmount = order.tipAmount,
            subtotalAmount = order.subtotalAmount,
            taxAmount = order.taxAmount,
            discountAmount = order.discountAmount,
            serviceChargeAmount = order.serviceChargeAmount,
            totalDue = order.totalDue,
            lines = new List<OrderLineDto>()
        };
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
    {
        _logger.LogInformation("Fetching order by ID {OrderId}", orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return null;
        }

        var lines = await _context.OrderLines.Where(l => l.orderId == orderId).ToListAsync();

        return new OrderDto
        {
            orderId = order.orderId,
            registrationNumber = order.registrationNumber,
            customerId = order.customerId,
            status = order.status,
            createdAt = order.createdAt,
            closedAt = order.closedAt,
            serviceChargePct = order.serviceChargePct,
            tipAmount = order.tipAmount,
            subtotalAmount = order.subtotalAmount,
            taxAmount = order.taxAmount,
            discountAmount = order.discountAmount,
            serviceChargeAmount = order.serviceChargeAmount,
            totalDue = order.totalDue,
            lines = lines.Select(l => new OrderLineDto
            {
                orderLineId = l.orderLineId,
                orderId = l.orderId,
                productId = l.productId,
                quantity = l.quantity,
                assignedStaffId = l.assignedStaffId,
                appointmentId = l.appointmentId,
                notes = l.notes,
                unitPrice = l.unitPrice,
                subTotal = l.subTotal
            }).ToList()
        };
    }

    public async Task<OrderDto?> UpdateOrderAsync(Guid orderId, OrderUpdateDto input)
    {
        _logger.LogInformation("Updating order {OrderId}", orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for update", orderId);
            return null;
        }

        if (order.status != OrderStatus.OPEN)
        {
            _logger.LogWarning("Cannot update closed order {OrderId}", orderId);
            throw new InvalidOperationException("Cannot update closed order");
        }

        if (input.customerId.HasValue)
            order.customerId = input.customerId.Value;

        await _context.SaveChangesAsync();

        var lines = await _context.OrderLines.Where(l => l.orderId == orderId).ToListAsync();

        _logger.LogInformation("Order {OrderId} updated successfully", orderId);

        return new OrderDto
        {
            orderId = order.orderId,
            registrationNumber = order.registrationNumber,
            customerId = order.customerId,
            status = order.status,
            createdAt = order.createdAt,
            closedAt = order.closedAt,
            serviceChargePct = order.serviceChargePct,
            tipAmount = order.tipAmount,
            subtotalAmount = order.subtotalAmount,
            taxAmount = order.taxAmount,
            discountAmount = order.discountAmount,
            serviceChargeAmount = order.serviceChargeAmount,
            totalDue = order.totalDue,
            lines = lines.Select(l => new OrderLineDto
            {
                orderLineId = l.orderLineId,
                orderId = l.orderId,
                productId = l.productId,
                quantity = l.quantity,
                assignedStaffId = l.assignedStaffId,
                appointmentId = l.appointmentId,
                notes = l.notes,
                unitPrice = l.unitPrice,
                subTotal = l.subTotal
            }).ToList()
        };
    }

    public async Task<OrderDto> CalculateOrderAsync(Guid orderId)
    {
        _logger.LogInformation("Calculating order totals for OrderId={OrderId}", orderId);

        var order = await _context.Orders.Include(o => o.Lines).FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for calculation", orderId);
            throw new KeyNotFoundException("Order not found");
        }

        decimal subtotal = order.Lines.Sum(l => l.subTotal);
        decimal totalTax = 0m;

        foreach (var line in order.Lines)
        {
            totalTax += await _orderLineService.CreateOrderLineTaxAsync(line.orderLineId);
        }

        var now = DateTime.UtcNow;
        var serviceChargeConfig = await _context.ServiceChargeConfigs
            .Where(sc => sc.registrationNumber == order.registrationNumber &&
                         sc.validFrom <= now &&
                         (sc.validTo == null || sc.validTo >= now))
            .OrderByDescending(sc => sc.validFrom)
            .FirstOrDefaultAsync();

        order.serviceChargePct = serviceChargeConfig?.percentage ?? 0m;

        decimal serviceChargeAmount = 0m;
        if (order.serviceChargePct > 0)
        {
            serviceChargeAmount = Math.Round(
                subtotal * (order.serviceChargePct / 100m),
                2,
                MidpointRounding.AwayFromZero
            );
        }

        decimal totalDue = subtotal + totalTax + serviceChargeAmount;

        order.subtotalAmount = subtotal;
        order.taxAmount = totalTax;
        order.serviceChargeAmount = serviceChargeAmount;
        order.totalDue = totalDue;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Order {OrderId} calculated: Subtotal={Subtotal}, Tax={Tax}, ServiceCharge={ServiceCharge}, TotalDue={TotalDue}",
            orderId, subtotal, totalTax, serviceChargeAmount, totalDue
        );

        return new OrderDto
        {
            orderId = order.orderId,
            registrationNumber = order.registrationNumber,
            customerId = order.customerId,
            status = order.status,
            createdAt = order.createdAt,
            closedAt = order.closedAt,
            serviceChargePct = order.serviceChargePct,
            tipAmount = order.tipAmount,
            subtotalAmount = order.subtotalAmount,
            taxAmount = order.taxAmount,
            serviceChargeAmount = order.serviceChargeAmount,
            totalDue = order.totalDue,
            lines = order.Lines.Select(l => new OrderLineDto
            {
                orderLineId = l.orderLineId,
                productId = l.productId,
                quantity = l.quantity,
                unitPrice = l.unitPrice,
                subTotal = l.subTotal
            }).ToList()
        };
    }

    public async Task<OrderDto> CloseOrderAsync(Guid orderId)
    {
        _logger.LogInformation("Closing order {OrderId}", orderId);

        var order = await _context.Orders.Include(o => o.Lines).FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for closing", orderId);
            throw new KeyNotFoundException("Order not found");
        }

        if (order.status == OrderStatus.CLOSED_PAID)
        {
            _logger.LogWarning("Order {OrderId} is already closed", orderId);
            throw new InvalidOperationException("Order is already closed");
        }

        order.status = OrderStatus.CLOSED_PAID;
        order.closedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Order {OrderId} closed successfully", orderId);

        return new OrderDto
        {
            orderId = order.orderId,
            registrationNumber = order.registrationNumber,
            customerId = order.customerId,
            status = order.status,
            createdAt = order.createdAt,
            closedAt = order.closedAt,
            serviceChargePct = order.serviceChargePct,
            tipAmount = order.tipAmount,
            subtotalAmount = order.subtotalAmount,
            taxAmount = order.taxAmount,
            serviceChargeAmount = order.serviceChargeAmount,
            totalDue = order.totalDue,
            lines = order.Lines.Select(l => new OrderLineDto
            {
                orderLineId = l.orderLineId,
                productId = l.productId,
                quantity = l.quantity,
                unitPrice = l.unitPrice,
                subTotal = l.subTotal
            }).ToList()
        };
    }
}
