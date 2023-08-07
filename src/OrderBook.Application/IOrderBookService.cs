using OrderBook.Domain;

namespace OrderBook.Application;

public interface IOrderBookService
{
    List<Order> CalculateOptimalStrategy(List<Order> orders, List<Account> accounts, OperationType operation, decimal amount);
}