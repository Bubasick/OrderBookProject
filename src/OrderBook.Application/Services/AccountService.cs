using OrderBook.Application.Exceptions;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;

namespace OrderBook.Application.Services;

public class AccountService : IAccountService
{
    public List<Account> ValidateAndFilterAccounts(List<Account> accounts, OperationType operation, decimal btcAmount)
    {
        var uniqueAccountIdsCount = accounts.Select(x => x.MetaExchangeId).Distinct().Count();

        if (uniqueAccountIdsCount != accounts.Count)
        {
            throw new EntityShouldBeUniqueException(nameof(Account));
        }

        switch (operation)
        {
            case OperationType.Buy:
                {
                    var result = accounts.Where(x => x.EuroBalance > 0).ToList();

                    if (!accounts.Any())
                    {
                        throw new BalanceTooLowException(btcAmount);
                    }

                    return result;
                }

            case OperationType.Sell:
                {
                    var result = accounts.Where(x => x.BtcBalance > 0).ToList();

                    if (!accounts.Any() || accounts.Sum(x => x.BtcBalance) < btcAmount)
                    {
                        throw new BalanceTooLowException(btcAmount);
                    }

                    return result;
                }

            default: return accounts;
        }
    }
}