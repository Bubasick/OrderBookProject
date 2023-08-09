using OrderBook.Domain.Entities;

namespace OrderBook.Tests;

public class TestDataHelper
{
    public static List<Order> GetFakeOrderList()
    {
        return new List<Order>()
        {
            new Order()
            {
                Id = 1m,
                Amount = 0.1m,
                Price = 2002,
                Type = OperationType.Buy
            },
            new Order()
            {
                Id = 2m,
                Amount = 0.2m,
                Price = 2000,
                Type = OperationType.Buy
            },
            new Order()
            {
                Id = 1m,
                Amount = 0.1m,
                Price = 2000,
                Type = OperationType.Sell
            },
            new Order()
            {
                Id = 2m,
                Amount = 0.2m,
                Price = 2002,
                Type = OperationType.Sell
            }
        };
    }

    public static List<Order> GetFakeOrderForSellList()
    {
        return new List<Order>()
        {
            new Order()
            {
                Id = 1m,
                Amount = 0.1m,
                Price = 2002,
                Type = OperationType.Buy
            },
            new Order()
            {
                Id = 2m,
                Amount = 0.2m,
                Price = 2000,
                Type = OperationType.Buy
            }
        };
    }

    public static List<Order> GetFakeOrderForBuyList()
    {
        return new List<Order>()
        {
            new Order()
            {
                Id = 1m,
                Amount = 0.1m,
                Price = 2000,
                Type = OperationType.Sell
            },
            new Order()
            {
                Id = 2m,
                Amount = 0.2m,
                Price = 2002,
                Type = OperationType.Sell
            }
        };
    }
}