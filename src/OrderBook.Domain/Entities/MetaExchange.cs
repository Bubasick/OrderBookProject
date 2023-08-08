namespace OrderBook.Domain.Entities
{
    public class MetaExchange
    {
        public decimal Id { get; set; }
        public DateTime AcqTime { get; set; }
        public List<OrderWrapper> Bids { get; set; }
        public List<OrderWrapper> Asks { get; set; }
    }
}