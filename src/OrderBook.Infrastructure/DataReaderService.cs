using Newtonsoft.Json;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;
using System.Globalization;

namespace OrderBook.Infrastructure;

public class DataReaderService : IDataReaderService
{
    public List<MetaExchange> GetData()
    {
        var path = Path.GetFullPath(@"C:/All/Projects//OrderBookProject/order_books_data");
        var lines = File.ReadAllLines(path);

        var result = new List<MetaExchange>();

        foreach (var line in lines)
        {
            var idStr = line.GetUntilOrEmpty("{").TrimEnd();

            var metaExchangeStr = line.Substring(line.IndexOf('{'));
            var metaExchange = JsonConvert.DeserializeObject<MetaExchange>(metaExchangeStr);

            metaExchange.Id = decimal.Parse(idStr, CultureInfo.InvariantCulture);
            metaExchange.Asks.ForEach(x => x.Order.Id = metaExchange.Id);
            metaExchange.Bids.ForEach(x => x.Order.Id = metaExchange.Id);

            result.Add(metaExchange);
        }
        return result;
    }
}