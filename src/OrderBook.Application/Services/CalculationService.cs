using OrderBook.Application.Exceptions;
using OrderBook.Application.Interfaces;
using OrderBook.Domain;
using OrderBook.Domain.Entities;

namespace OrderBook.Application.Services;

public class CalculationService : ICalculationService
{
    private readonly IOrderService _orderService;

    private readonly IAccountService _accountService;

    public CalculationService(IOrderService orderService, IAccountService accountService)
    {
        _orderService = orderService;
        _accountService = accountService;
    }

    public List<Order> CalculateOptimalStrategy(List<Account> accounts, OperationType operation, decimal amount)
    {
        accounts = _accountService.ValidateAndFilterAccounts(accounts, operation, amount);

        switch (operation)
        {
            case OperationType.Buy:
                {
                    var orders = _orderService.GetOrdersForBuys(accounts.Select(x => x.MetaExchangeId));
                    return CalculateOptimalBuys(orders, accounts, amount);
                }

            case OperationType.Sell:
                {
                    var orders = _orderService.GetOrdersForSells(accounts.Select(x => x.MetaExchangeId));
                    return CalculateOptimalSells(orders, accounts, amount);
                }

            default: return new List<Order>();
        }
    }

    private List<Order> CalculateOptimalBuys(List<Order> orders, List<Account> accounts, decimal btcBuyAmount)
    {
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

            var buyValueInEuro = decimal.Multiply(order.Price, howMuchCanBuy).Round(2);
            account.EuroBalance = decimal.Subtract(account.EuroBalance, buyValueInEuro);
            btcBuyAmount = decimal.Subtract(btcBuyAmount, howMuchCanBuy).Round(8);

            if (account.EuroBalance == 0)
            {
                accounts.Remove(account);
            }
            else if (account.EuroBalance < 0)
            {
                throw new CriticalCalculationErrorException(account.MetaExchangeId);
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

        ValidateAmountAfterCalculation(btcBuyAmount);

        return result;
    }

    private List<Order> CalculateOptimalSells(List<Order> orders, List<Account> accounts, decimal btcSellAmount)
    {
        var result = new List<Order>();

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

            account.BtcBalance = decimal.Subtract(account.BtcBalance, howMuchBtcCanSell);
            btcSellAmount = decimal.Subtract(btcSellAmount, howMuchBtcCanSell);

            if (account.BtcBalance == 0)
            {
                accounts.Remove(account);
            }
            else if (account.BtcBalance < 0)
            {
                throw new CriticalCalculationErrorException(account.MetaExchangeId);
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

        ValidateAmountAfterCalculation(btcSellAmount);

        return result;
    }

    private void ValidateAmountAfterCalculation(decimal amount)
    {
        if (amount > 0)
        {
            throw new RequestExceedsMarketException(amount);
        }
    }
}