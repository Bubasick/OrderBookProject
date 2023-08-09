using OrderBook.Application.Exceptions;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;

namespace OrderBook.Application.Services;

public class OrderService : IOrderService
{
    private readonly IDataReaderService _dataReaderService;

    public OrderService(IDataReaderService dataReaderService)
    {
        _dataReaderService = dataReaderService;
    }

    private List<Order> GetOrders(IEnumerable<decimal> ids)
    {
        var result = _dataReaderService.GetOrders()
            .Where(order => order.Id.HasValue && ids.Contains(order.Id.Value))
            .ToList();

        return result;
    }

    public List<Order> GetOrdersForBuys(IEnumerable<decimal> ids)
    {
        var result = GetOrders(ids)
            .Where(x => x.Type == OperationType.Sell)
            .OrderBy(x => x.Price)
            .ToList();

        ValidateOrders(result);

        return result;
    }

    public List<Order> GetOrdersForSells(IEnumerable<decimal> ids)
    {
        var result = GetOrders(ids)
            .Where(x => x.Type == OperationType.Buy)
            .OrderByDescending(x => x.Price)
            .ToList();

        ValidateOrders(result);

        return result;
    }

    private void ValidateOrders(IEnumerable<Order> orders)
    {
        if (!orders.Any())
        {
            throw new NotFoundException(nameof(Order));
        }
    }
}