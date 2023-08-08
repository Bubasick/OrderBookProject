namespace OrderBook.Domain.Entities;

public class Order
{
    public decimal? Id { get; set; }
    public DateTime Time { get; set; }
    public OperationType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

}