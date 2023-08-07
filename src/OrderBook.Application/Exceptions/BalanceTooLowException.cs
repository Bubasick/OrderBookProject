using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class BalanceTooLowException : Exception
{
    public BalanceTooLowException(decimal buyAmount) : base($"Your balance does not allow to perform this type of operation with the specified amount: {buyAmount}") { }

    public BalanceTooLowException(string message) : base(message) { }

    public BalanceTooLowException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}