using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.orders.dto;
using Pos.Api.Orders.Model;

namespace Pos.Api.orders.service;

public interface IOrderLineService
{
    Task<OrderLineDto> AddOrderLineAsync(Guid orderId, OrderLineCreateDto input);
    
    Task<OrderLine> UpdateLineAsync(Guid orderId, Guid lineId, OrderLineUpdateDto dto);

    Task DeleteLineAsync(Guid orderId, Guid lineId);
}
    
    
public class OrderLineService : IOrderLineService
{
    private readonly AppDbContext _context;
    
    public OrderLineService(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<OrderLineDto> AddOrderLineAsync(Guid orderId, OrderLineCreateDto input)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found");

        if (order.status != OrderStatus.OPEN)
            throw new InvalidOperationException("Cannot add line to closed order");
        
        
        var line = new OrderLine
        {
            orderLineId = Guid.NewGuid(),
            orderId = orderId,
            productId = input.productId,
            quantity = input.quantity,
            assignedStaffId = input.assignedStaffId,
            appointmentId = input.appointmentId,
            notes = input.notes,
            unitPrice = 0, // TODO: fetch product price
            subTotal = 0   // TODO: calculate quantity * unitPrice + options
        };

        await _context.OrderLines.AddAsync(line);
        await _context.SaveChangesAsync();
        
        var lineDto = new OrderLineDto
        {
            orderLineId = line.orderLineId,
            orderId = line.orderId,
            productId = line.productId,
            quantity = line.quantity,
            assignedStaffId = line.assignedStaffId,
            appointmentId = line.appointmentId,
            notes = line.notes,
            unitPrice = line.unitPrice,
            subTotal = line.subTotal,
            //options = new List<OrderLineOptionDto>()
        };

        return lineDto;
    }

    public async Task<OrderLine> UpdateLineAsync(Guid orderId, Guid lineId, OrderLineUpdateDto dto)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found.");

        if (order.status != OrderStatus.OPEN)
            throw new InvalidOperationException("Cannot update a line on a closed order.");
        
        var line = await _context.OrderLines.FirstOrDefaultAsync(l => l.orderLineId == lineId && l.orderId == orderId);
        if (line == null)
            throw new KeyNotFoundException("Order line not found.");
        
        if (dto.quantity.HasValue)
            line.quantity = dto.quantity.Value;

        if (dto.assignedStaffId.HasValue)
            line.assignedStaffId = dto.assignedStaffId.Value;

        if (dto.notes != null)
            line.notes = dto.notes;
        
        line.subTotal = line.unitPrice * line.quantity;

        await _context.SaveChangesAsync();

        return line;
    }
    
    public async Task DeleteLineAsync(Guid orderId, Guid lineId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found.");

        if (order.status != OrderStatus.OPEN)
            throw new InvalidOperationException("Cannot delete a line on a closed order.");
        
        var line = await _context.OrderLines
            .FirstOrDefaultAsync(l => l.orderLineId == lineId && l.orderId == orderId);

        if (line == null)
            throw new KeyNotFoundException("Order line not found.");
        
        _context.OrderLines.Remove(line);
        await _context.SaveChangesAsync();
    }
}
