using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<OrderLineService> _logger;

    public OrderLineService(AppDbContext dbContext, ILogger<OrderLineService> logger)
    {
        _context = dbContext;
        _logger = logger;
    }

    public async Task<OrderLineDto> AddOrderLineAsync(Guid orderId, OrderLineCreateDto input)
    {
        _logger.LogInformation("Adding new order line to OrderId={OrderId}, ProductId={ProductId}, Qty={Qty}",
            orderId, input.productId, input.quantity);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for adding line", orderId);
            throw new KeyNotFoundException("Order not found");
        }

        if (order.status != OrderStatus.OPEN)
        {
            _logger.LogWarning("Cannot add line to closed order {OrderId}", orderId);
            throw new InvalidOperationException("Cannot add line to closed order");
        }

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

        _logger.LogInformation("Order line added: OrderLineId={LineId}, SubTotal={SubTotal}", line.orderLineId, line.subTotal);

        return new OrderLineDto
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
    }

    public async Task<OrderLine> UpdateLineAsync(Guid orderId, Guid lineId, OrderLineUpdateDto dto)
    {
        _logger.LogInformation("Updating order line {LineId} for OrderId={OrderId}", lineId, orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for updating line {LineId}", orderId, lineId);
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.status != OrderStatus.OPEN)
        {
            _logger.LogWarning("Cannot update line {LineId} on closed order {OrderId}", lineId, orderId);
            throw new InvalidOperationException("Cannot update a line on a closed order.");
        }

        var line = await _context.OrderLines.FirstOrDefaultAsync(l => l.orderLineId == lineId && l.orderId == orderId);
        if (line == null)
        {
            _logger.LogWarning("Order line {LineId} not found in order {OrderId}", lineId, orderId);
            throw new KeyNotFoundException("Order line not found.");
        }

        if (dto.quantity.HasValue)
            line.quantity = dto.quantity.Value;
        if (dto.assignedStaffId != null)
            line.assignedStaffId = dto.assignedStaffId;
        if (dto.notes != null)
            line.notes = dto.notes;

        line.subTotal = line.unitPrice * line.quantity;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Order line updated: OrderLineId={LineId}, NewSubTotal={SubTotal}", line.orderLineId, line.subTotal);

        return line;
    }

    public async Task DeleteLineAsync(Guid orderId, Guid lineId)
    {
        _logger.LogInformation("Deleting order line {LineId} from OrderId={OrderId}", lineId, orderId);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found for deleting line {LineId}", orderId, lineId);
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.status != OrderStatus.OPEN)
        {
            _logger.LogWarning("Cannot delete line {LineId} from closed order {OrderId}", lineId, orderId);
            throw new InvalidOperationException("Cannot delete a line on a closed order.");
        }

        var line = await _context.OrderLines.FirstOrDefaultAsync(l => l.orderLineId == lineId && l.orderId == orderId);
        if (line == null)
        {
            _logger.LogWarning("Order line {LineId} not found for deletion in order {OrderId}", lineId, orderId);
            throw new KeyNotFoundException("Order line not found.");
        }

        _context.OrderLines.Remove(line);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order line {LineId} deleted successfully", lineId);
    }

    public async Task<decimal> CreateOrderLineTaxAsync(Guid orderLineId)
    {
        _logger.LogInformation("Calculating tax for OrderLineId={LineId}", orderLineId);

        var orderLine = await _context.OrderLines.FirstOrDefaultAsync(ol => ol.orderLineId == orderLineId);
        if (orderLine == null)
        {
            _logger.LogWarning("OrderLine {LineId} not found for tax calculation", orderLineId);
            throw new KeyNotFoundException("OrderLine not found");
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.productId == orderLine.productId);
        if (product == null)
        {
            _logger.LogWarning("Product {ProductId} not found for OrderLine {LineId}", orderLine.productId, orderLineId);
            throw new KeyNotFoundException("Product not found");
        }

        var tax = await _context.Taxes.FirstOrDefaultAsync(t => t.id == product.taxCode);
        if (tax == null || tax.percentage <= 0)
        {
            _logger.LogInformation("No tax applicable for OrderLine {LineId}", orderLineId);
            return 0m;
        }

        var taxAmount = Math.Round(orderLine.subTotal * (tax.percentage / 100m), 2, MidpointRounding.AwayFromZero);

        var orderLineTax = await _context.OrderLineTaxes.FirstOrDefaultAsync(olt => olt.orderLineId == orderLineId);
        if (orderLineTax != null)
        {
            orderLineTax.taxCode = tax.id;
            orderLineTax.taxPercentage = tax.percentage;
            orderLineTax.taxAmount = taxAmount;
            _context.OrderLineTaxes.Update(orderLineTax);

            _logger.LogInformation("Updated tax for OrderLine {LineId}: TaxAmount={TaxAmount}", orderLineId, taxAmount);
        }
        else
        {
            orderLineTax = new OrderLineTax
            {
                orderLineTaxId = Guid.NewGuid(),
                orderLineId = orderLineId,
                taxCode = tax.id,
                taxPercentage = tax.percentage,
                taxAmount = taxAmount
            };

            _context.OrderLineTaxes.Add(orderLineTax);

            _logger.LogInformation("Created tax for OrderLine {LineId}: TaxAmount={TaxAmount}", orderLineId, taxAmount);
        }

        await _context.SaveChangesAsync();
        return taxAmount;
    }
}
