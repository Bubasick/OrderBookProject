using Newtonsoft.Json;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;
using System.Globalization;

namespace OrderBook.Infrastructure;

public class DataReaderService : IDataReaderService
{
    private readonly List<Order> _orders;

    public DataReaderService()
    {
        _orders = GetOrdersFromFile();
    }

    private List<Order> GetOrdersFromFile()
    {
        var path = Path.GetFullPath(@"C:/All/Projects//OrderBookProject/order_books_data");
        var lines = File.ReadAllLines(path);

        var result = new List<Order>();

        foreach (var line in lines)
        {
            var idStr = line.GetUntilOrEmpty("{").TrimEnd();

            var metaExchangeStr = line.Substring(line.IndexOf('{'));
            var metaExchange = JsonConvert.DeserializeObject<MetaExchange>(metaExchangeStr);

            metaExchange.Id = decimal.Parse(idStr, CultureInfo.InvariantCulture);
            metaExchange.Asks.ForEach(x => x.Order.Id = metaExchange.Id);
            metaExchange.Bids.ForEach(x => x.Order.Id = metaExchange.Id);

            var bids = metaExchange.Bids.Select(x => x.Order).ToList();
            var asks = metaExchange.Asks.Select(x => x.Order).ToList();
            result.AddRange(bids);
            result.AddRange(asks);
        }
        return result;
    }

    public virtual List<Order> GetOrders()
    {
        return _orders.ConvertAll(order => new Order(order.Id, order.Type, order.Amount, order.Price));
    }
}