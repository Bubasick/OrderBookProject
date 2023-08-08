namespace OrderBook.Domain;

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

public class AccountComparer : IEqualityComparer<Account>
{
    public bool Equals(Account x, Account y) => x?.MetaExchangeId == y?.MetaExchangeId;
    public int GetHashCode(Account obj) => obj.MetaExchangeId.GetHashCode();
}