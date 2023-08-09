using OrderBook.Domain.Entities;

namespace OrderBook.Application.Interfaces;

public interface ICalculationService
{
    List<Order> CalculateOptimalStrategy(List<Account> accounts, OperationType operation, decimal amount);
}