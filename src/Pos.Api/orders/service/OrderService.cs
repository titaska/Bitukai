using Microsoft.EntityFrameworkCore;
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
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IOrderLineService _orderLineService;
    
    public OrderService(AppDbContext dbContext, IOrderLineService orderLineService)
    {
        _context = dbContext;
        _orderLineService = orderLineService;
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
        var query = _context.Orders
            .Include(o => o.Lines)
            .AsQueryable();
        

        if (status.HasValue)
            query = query.Where(o => o.status == status.Value);

        if (customerId.HasValue)
            query = query.Where(o => o.customerId == customerId.Value);

        if (fromDate.HasValue)
            query = query.Where(o => o.createdAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.createdAt <= toDate.Value);

        int total = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.createdAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        
        
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
                subTotal = l.subTotal,
                //options = new List<OrderLineOptionDto>(), //empty for now
            }).ToList(),
        }).ToList();

        return (orderDtos, total);
    }
    
    
    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto input)
    {
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
        
        var orderDto = new OrderDto
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

        return orderDto;
    }
    
    
    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);

        if (order == null)
            return null;
        
        var lines = await _context.OrderLines
            .Where(l => l.orderId == orderId)
            .ToListAsync();

        var orderDto = new OrderDto
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
                subTotal = l.subTotal,
                //options = new List<OrderLineOptionDto>(), //empty for now
            }).ToList(),
        };

        return orderDto;
    }
    
    public async Task<OrderDto?> UpdateOrderAsync(Guid orderId, OrderUpdateDto input)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);

        if (order == null)
            return null;

        if (order.status != OrderStatus.OPEN)
            throw new InvalidOperationException("Cannot update closed order");


        if (input.customerId.HasValue)
            order.customerId = input.customerId.Value;
        
        
        await _context.SaveChangesAsync();
        
        var lines = await _context.OrderLines
            .Where(l => l.orderId == orderId)
            .ToListAsync();

        var orderDto = new OrderDto
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
            }).ToList(),
        };

        return orderDto;
    }

    public async Task<OrderDto> CloseOrderAsync(Guid orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.orderId == orderId);

        if (order == null)
            throw new KeyNotFoundException("Order not found");

        if (order.status == OrderStatus.CLOSED_PAID)
            throw new InvalidOperationException("Order is already closed");
        
        decimal subtotal = order.Lines.Sum(l => l.subTotal);
        
        decimal totalTax = 0m;
        foreach (var line in order.Lines)
        {
            totalTax += await _orderLineService.CreateOrderLineTaxAsync(line.orderLineId);
        }
        
        decimal serviceChargeAmount = 0m;
        if (order.serviceChargePct > 0)
        {
            serviceChargeAmount = Math.Round(
                subtotal * (order.serviceChargePct / 100m),
                2,
                MidpointRounding.AwayFromZero
            );
        }

        decimal totalDue =
            subtotal +
            totalTax +
            serviceChargeAmount;
        
        order.subtotalAmount = subtotal;
        order.taxAmount = totalTax;
        order.serviceChargeAmount = serviceChargeAmount;
        order.totalDue = totalDue;
        order.status = OrderStatus.CLOSED_PAID;
        order.closedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
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
