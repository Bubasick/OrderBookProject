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

        for (int i = 0; i < orders.Count; i++)
        {
            if (btcBuyAmount <= 0)
            {
                break;
            }

            var order = orders[i];
            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

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
                orders.RemoveAll(x=> x.Id == account.MetaExchangeId);
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

        PostCalculationValidation(btcBuyAmount, accounts, OperationType.Buy);

        return result;
    }

    private List<Order> CalculateOptimalSells(List<Order> orders, List<Account> accounts, decimal btcSellAmount)
    {
        var result = new List<Order>();


        for (int i = 0; i < orders.Count; i++)
        {
            if (btcSellAmount <= 0)
            {
                return result;
            }

            var order = orders[i];

            var account = accounts.FirstOrDefault(x => x.MetaExchangeId == order.Id);

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
                orders.RemoveAll(x => x.Id == account.MetaExchangeId);
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

        PostCalculationValidation(btcSellAmount, accounts, OperationType.Sell);

        return result;
    }

    private void PostCalculationValidation(decimal amount, List<Account> accounts, OperationType operation)
    {
        if (amount > 0)
        {
            switch (operation)
            {
                case OperationType.Buy:
                {
                    _accountService.CheckIfEuroBalanceEmpty(accounts);
                    break;
                }

                case OperationType.Sell:
                {
                    _accountService.CheckIfBtcBalanceEmpty(accounts);
                    break;
                }
            }

            throw new RequestExceedsMarketException(amount);
        }
    }
}