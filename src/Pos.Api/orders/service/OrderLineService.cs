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

    Task<decimal> CreateOrderLineTaxAsync(Guid orderLineId);
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
            unitPrice = input.unitPrice,
            subTotal = input.unitPrice * input.quantity
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

        if (dto.assignedStaffId != null)
            line.assignedStaffId = dto.assignedStaffId;

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

    public async Task<decimal> CreateOrderLineTaxAsync(Guid orderLineId)
    {
        // Get the order line
        var orderLine = await _context.OrderLines
            .FirstOrDefaultAsync(ol => ol.orderLineId == orderLineId);

        if (orderLine == null)
            throw new KeyNotFoundException("OrderLine not found");

        // Get the product
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.productId == orderLine.productId);

        if (product == null)
            throw new KeyNotFoundException("Product not found");

        // Get the tax
        var tax = await _context.Taxes
            .FirstOrDefaultAsync(t => t.id == product.taxCode);

        if (tax == null || tax.percentage <= 0)
            return 0m;

        // Calculate tax amount
        var taxAmount = Math.Round(
            orderLine.subTotal * (tax.percentage / 100m),
            2,
            MidpointRounding.AwayFromZero
        );

        // Check if OrderLineTax already exists
        var orderLineTax = await _context.OrderLineTaxes
            .FirstOrDefaultAsync(olt => olt.orderLineId == orderLineId);

        if (orderLineTax != null)
        {
            // Update existing record
            orderLineTax.taxCode = tax.id;
            orderLineTax.taxPercentage = tax.percentage;
            orderLineTax.taxAmount = taxAmount;

            _context.OrderLineTaxes.Update(orderLineTax);
        }
        else
        {
            // Create new record
            orderLineTax = new OrderLineTax
            {
                orderLineTaxId = Guid.NewGuid(),
                orderLineId = orderLineId,
                taxCode = tax.id,
                taxPercentage = tax.percentage,
                taxAmount = taxAmount
            };

            _context.OrderLineTaxes.Add(orderLineTax);
        }

        await _context.SaveChangesAsync();

        return taxAmount;
    }

}
