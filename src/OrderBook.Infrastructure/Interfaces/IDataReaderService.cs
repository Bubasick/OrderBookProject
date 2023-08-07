using OrderBook.Domain;

namespace OrderBook.Infrastructure.Interfaces;

public interface IDataReaderService
{
    List<MetaExchange> GetData();
}