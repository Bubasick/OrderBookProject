using Azure.Core;
using OrderBook.Application.Exceptions;
using OrderBook.Domain;
using OrderBook.Infrastructure;
using OrderBook.Infrastructure.Interfaces;

namespace OrderBook.Application;

public class OrderBookService : IOrderBookService
{
    public OrderBookService()
    {
    }

    public List<Order> CalculateOptimalStrategy(List<Order> orders,List<Account> accounts, OperationType operation, decimal amount)
    {
        ValidateAccounts(accounts);

        switch (operation)
        {
            case OperationType.Buy:
            {
                return CalculateOptimalBuys(orders, accounts, amount);
            }
            
            case OperationType.Sell:
            {
                return CalculateOptimalSells(orders, accounts, amount);
            }

            default: return new List<Order>();
        }
    }

    private List<Order> CalculateOptimalBuys(List<Order> orders, List<Account> accounts, decimal buyAmount)
    {
        var accountIdList = accounts.Select(x => x.MetaExchangeId).ToList();
        
        orders = orders
            .Where(order => order.Id.HasValue && accountIdList.Contains(order.Id.Value))
            .Where(x=> x.Type == OperationType.Sell)
            .OrderBy(x => x.Price).ToList();

        if (!orders.Any())
        {
            throw new NotFoundException(nameof(Order));
        }
        var result = new List<Order>();

        foreach (var order in orders)
        {
            if (buyAmount <= 0)
            {
                return result;
            }

            if (!accounts.Any())
            {
                throw new BalanceTooLowException(buyAmount);
            }

            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

            if (account == null)
            {
                continue;
            }

            decimal howMuchCanBuy = account.EuroBalance / order.Price;


            howMuchCanBuy = new List<decimal>()
            {
                howMuchCanBuy,
                buyAmount,
                order.Amount
            }.Min();

            var buyValue = order.Price * howMuchCanBuy;
            account.EuroBalance -= buyValue;
            account.BtcBalance += howMuchCanBuy;
            buyAmount -= howMuchCanBuy;

            if (account.EuroBalance <= 0)
            {
                accounts.Remove(account);
            }

            var resultOrder = new Order()
            {
                Id = order.Id,
                Amount = howMuchCanBuy,
                Price = order.Price,
                Type = OperationType.Buy

            };

            result.Add(resultOrder);
        }
        
        if (buyAmount > 0)
        {
            throw new RequestExceedsMarketException(buyAmount);
        }

        return result;
    }

    private List<Order> CalculateOptimalSells(List<Order> orders, List<Account> accounts, decimal sellAmount)
    {
        var accountIdList = accounts.Select(x => x.MetaExchangeId).ToList();
        
        orders = orders
            .Where(order => order.Id.HasValue && accountIdList.Contains(order.Id.Value))
            .Where(x => x.Type == OperationType.Buy)
            .OrderByDescending(x => x.Price).ToList(); ;

        var result = new List<Order>();

        if (!orders.Any())
        {
            throw new NotFoundException(nameof(Order));
        }
        

        foreach (var order in orders)
        {
            if (sellAmount <= 0)
            {
                return result;
            }

            if (!accounts.Any())
            {
                throw new BalanceTooLowException(sellAmount);
            }

            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

            if (account == null)
            {
                continue;
            }

            decimal howMuchCanSell = new List<decimal>()
            {
                account.BtcBalance,
                sellAmount,
                order.Amount
            }.Min();

            var sellValue = order.Price * howMuchCanSell;
            account.EuroBalance += sellValue;
            account.BtcBalance -= howMuchCanSell;
            sellAmount -= howMuchCanSell;

            if (account.BtcBalance <= 0) 
            {
                accounts.Remove(account);
            }

            var resultOrder = new Order()
            {
                Id = order.Id,
                Amount = howMuchCanSell,
                Price = order.Price,
                Type = OperationType.Sell

            };

            result.Add(resultOrder);
        }

        if (sellAmount > 0)
        {
            throw new RequestExceedsMarketException(sellAmount);
        }

        return result;
    }

    private void ValidateAccounts(List<Account> accounts)
    {
        var uniqueAccountIdsCount = accounts.Select(x => x.MetaExchangeId).Distinct().Count();

        if (uniqueAccountIdsCount != accounts.Count)
        {
            throw new EntityShouldBeUniqueException(nameof(Account));
        }
    }
}