using OrderBook.Domain.Entities;

namespace OrderBook.Application.Interfaces;

public interface IDataReaderService
{
    List<Order> GetOrders();
}