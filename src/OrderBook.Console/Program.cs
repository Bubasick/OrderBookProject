// See https://aka.ms/new-console-template for more information

using OrderBook.Application;
using OrderBook.Domain.Entities;
using OrderBook.Infrastructure;
using System.Globalization;
using OrderBook.Application.Services;

Console.WriteLine("Please, specify the type of operation you want to perform:");
var operationType = (OperationType)Enum.Parse(typeof(OperationType), Console.ReadLine());

Console.WriteLine("Please, specify the amount of btc you want to sell / buy:");
var btcAmount = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

Console.WriteLine("Please, specify the amount of accounts you have:");
var accountAmount = int.Parse(Console.ReadLine());

var accounts = new List<Account>();

for (var i = 0; i < accountAmount; i++)
{
    var account = new Account();
    Console.WriteLine("Please, specify account id (equals to metaExchange id):");
    account.MetaExchangeId = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

    if (operationType == OperationType.Sell)
    {
        Console.WriteLine("Please, specify the amount of btc you have on this account:");
        account.BtcBalance = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
    }

    if (operationType == OperationType.Buy)
    {
        Console.WriteLine("Please, specify the amount of euro you have on this account:");
        account.EuroBalance = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
    }

    accounts.Add(account);
}

var dataReaderService = new DataReaderService();
var orderService = new OrderService(dataReaderService);
var accountService = new AccountService();
var orderBookService = new CalculationService(orderService, accountService);
var result = new List<Order>();

try
{
    result = orderBookService.CalculateOptimalStrategy(accounts, operationType, btcAmount);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

foreach (var order in result)
{
    Console.WriteLine($"Order id: {order.Id}");
    Console.WriteLine($"Order type: {order.Type}");
    Console.WriteLine($"Order amount: {order.Amount}");
    Console.WriteLine($"Order price: {order.Price}");
}