using OrderBook.Domain.Entities;

namespace OrderBook.Application.Interfaces;

public interface IOrderService
{
    List<Order> GetOrdersForBuys(IEnumerable<decimal> ids);
    List<Order> GetOrdersForSells(IEnumerable<decimal> ids);
}