using OrderBook.Domain.Entities;

namespace OrderBook.Infrastructure.Interfaces;

public interface IDataReaderService
{
    List<MetaExchange> GetData();
}