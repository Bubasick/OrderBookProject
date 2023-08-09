using OrderBook.Application.Exceptions;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;

namespace OrderBook.Application.Services;

public class AccountService : IAccountService
{
    public virtual List<Account>  ValidateAndFilterAccounts(List<Account> accounts, OperationType operation, decimal btcAmount)
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

                if (!result.Any())
                {
                    throw new BalanceTooLowException(btcAmount);
                }

                return result;
            }

            case OperationType.Sell:
            {
                var result = accounts.Where(x => x.BtcBalance > 0).ToList();

                if (!result.Any() || result.Sum(x => x.BtcBalance) < btcAmount)
                {
                    throw new BalanceTooLowException(btcAmount);
                }

                return result;
            }

            default: return accounts;
        }
    }
    public virtual void CheckIfBtcBalanceEmpty(List<Account> accounts)
    {
        if (accounts.TrueForAll(x => x.BtcBalance == 0))
        {
            throw new BalanceTooLowException();
        };
    }
    public virtual void CheckIfEuroBalanceEmpty(List<Account> accounts)
    {
        if (accounts.TrueForAll(x => x.EuroBalance == 0))
        {
            throw new BalanceTooLowException();
        };
    }
}