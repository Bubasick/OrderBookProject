using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class RequestExceedsMarketException : Exception
{
    public RequestExceedsMarketException(decimal buyAmount) : base($"Your request exceeds the market available amount by: {buyAmount} btc")
    {
    }

    public RequestExceedsMarketException(string message) : base(message)
    {
    }

    public RequestExceedsMarketException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}