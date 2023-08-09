namespace OrderBook.Domain.Entities;

public class Account
{
    private decimal _euroBalance;
    private decimal _btcBalance;

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

    public decimal BtcBalance
    {
        get => _btcBalance;
        set => _btcBalance = value.Round(8);
    }

    public decimal EuroBalance
    {
        get => _euroBalance;
        set => _euroBalance = value.Round(2);
    }
}