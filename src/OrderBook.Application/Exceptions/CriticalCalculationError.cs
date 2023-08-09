using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class CriticalCalculationError : Exception
{
    public CriticalCalculationError(decimal accountId) : base($"A critical calculation error occurred while processing your request. Account id:{accountId}")
    {
    }

    public CriticalCalculationError(string message) : base(message)
    {
    }

    public CriticalCalculationError(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}