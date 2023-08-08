using OrderBook.Domain.Entities;

namespace OrderBook.Application.Interfaces;

public interface IOrderBookService
{
    List<Order> CalculateOptimalStrategy(List<Order> orders, List<Account> accounts, OperationType operation, decimal amount);
}