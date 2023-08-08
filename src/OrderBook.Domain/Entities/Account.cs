namespace OrderBook.Domain.Entities;

public class Account
{
    public Account()
    {
    }

    public Account(decimal Id, decimal btcBalance, decimal euroBalance)
    {
        MetaExchangeId = Id;
        BtcBalance = btcBalance;
        EuroBalance = euroBalance;
    }

    public decimal MetaExchangeId { get; set; }
    public decimal BtcBalance { get; set; }
    public decimal EuroBalance { get; set; }
}