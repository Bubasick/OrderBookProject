using System.Globalization;
using Newtonsoft.Json;
using OrderBook.Domain.Entities;
using OrderBook.Infrastructure.Interfaces;

namespace OrderBook.Infrastructure;

public class DataReaderService : IDataReaderService
{
    public List<MetaExchange> GetData()
    {
        var path = @"C:/All/Projects//OrderBookProject/order_books_data";
        var lines = File.ReadAllLines(path);

        var result = new List<MetaExchange>();

        
        foreach (var line in lines)
        {
            var idStr =  Helper.GetUntilOrEmpty(line, "{").TrimEnd();

            var metaExchangeStr = line.Substring(line.IndexOf('{'));
            var metaEchange  = JsonConvert.DeserializeObject<MetaExchange>(metaExchangeStr);
            metaEchange.Id = decimal.Parse(idStr, CultureInfo.InvariantCulture);
            metaEchange.Asks.ForEach(x=> x.Order.Id = metaEchange.Id);
            metaEchange.Bids.ForEach(x => x.Order.Id = metaEchange.Id);
            result.Add(metaEchange);
        }

        return result;
    }

}