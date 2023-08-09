using OrderBook.Application.Exceptions;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;
using OrderBook.Domain;
namespace OrderBook.Application;

public class OrderBookService : IOrderBookService
{
    private readonly IDataReaderService _dataReaderService;
    public OrderBookService(IDataReaderService dataReaderService)
    {
        _dataReaderService = dataReaderService;
    }
    public List<Order> CalculateOptimalStrategy(List<Account> accounts, OperationType operation, decimal amount)
    {
        var orders = _dataReaderService.GetOrders();

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

    private List<Order> CalculateOptimalBuys(List<Order> orders, List<Account> accounts, decimal btcBuyAmount)
    {
        accounts = accounts.Where(x => x.EuroBalance > 0).ToList();

        if (!accounts.Any())
        {
            throw new BalanceTooLowException(btcBuyAmount);
        }

        var accountIdList = accounts.Select(x => x.MetaExchangeId).ToList();

        orders = orders
            .Where(order => order.Id.HasValue && accountIdList.Contains(order.Id.Value))
            .Where(x => x.Type == OperationType.Sell)
            .OrderBy(x => x.Price).ToList();

        if (!orders.Any())
        {
            throw new NotFoundException(nameof(Order));
        }

        var result = new List<Order>();

        foreach (var order in orders)
        {
            if (btcBuyAmount <= 0)
            {
                return result;
            }

            if (!accounts.Any())
            {
                throw new BalanceTooLowException(btcBuyAmount);
            }

            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

            if (account == null)
            {
                continue;
            }
            

            var howMuchCanBuy = new List<decimal>()
            {
                decimal.Divide(account.EuroBalance, order.Price).Round(8, MidpointRounding.ToNegativeInfinity),
                btcBuyAmount,
                order.Amount
            }.Min();
            
            var buyValueInEuro =  decimal.Multiply(order.Price, howMuchCanBuy).Round(2);
            account.EuroBalance = decimal.Subtract(account.EuroBalance, buyValueInEuro);
            account.BtcBalance = decimal.Add(account.BtcBalance, howMuchCanBuy);
            btcBuyAmount = decimal.Subtract(btcBuyAmount, howMuchCanBuy).Round(8);

            if (account.EuroBalance == 0)
            {
                accounts.Remove(account);
            }

            else if (account.EuroBalance < 0)
            {
                throw new CriticalCalculationError(account.MetaExchangeId);
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

        if (btcBuyAmount > 0)
        {
            throw new RequestExceedsMarketException(btcBuyAmount);
        }

        return result;
    }

    private List<Order> CalculateOptimalSells(List<Order> orders, List<Account> accounts, decimal btcSellAmount)
    {
        accounts = accounts.Where(x => x.BtcBalance > 0).ToList();

        if (!accounts.Any() || accounts.Sum(x=> x.BtcBalance) < btcSellAmount)
        {
            throw new BalanceTooLowException(btcSellAmount);
        }

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
            if (btcSellAmount <= 0)
            {
                return result;
            }

            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

            if (account == null)
            {
                continue;
            }

            var howMuchBtcCanSell = new List<decimal>()
            {
                account.BtcBalance,
                btcSellAmount,
                order.Amount
            }.Min();

            account.BtcBalance = decimal.Subtract(account.BtcBalance,howMuchBtcCanSell);
            btcSellAmount = decimal.Subtract(btcSellAmount, howMuchBtcCanSell);

            if (account.BtcBalance == 0)
            {
                accounts.Remove(account);
            }

            else if (account.BtcBalance < 0)
            {
                throw new CriticalCalculationError(account.MetaExchangeId);
            }

            var resultOrder = new Order()
            {
                Id = order.Id,
                Amount = howMuchBtcCanSell,
                Price = order.Price,
                Type = OperationType.Sell
            };

            result.Add(resultOrder);
        }

        if (btcSellAmount > 0)
        {
            throw new RequestExceedsMarketException(btcSellAmount);
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