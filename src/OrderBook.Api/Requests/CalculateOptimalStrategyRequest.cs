using OrderBook.Domain.Entities;

namespace OrderBook.Api.Requests
{
    public class CalculateOptimalStrategyRequest
    {
        public List<Account> Accounts { get; set; }

        public List<Order> Orders { get; set; }
    }
}