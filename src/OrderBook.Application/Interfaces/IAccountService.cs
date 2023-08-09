using OrderBook.Domain.Entities;

namespace OrderBook.Application.Interfaces;

public interface IAccountService
{
    List<Account> ValidateAndFilterAccounts(List<Account> accounts, OperationType operation, decimal btcAmount);
}