using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class CriticalCalculationErrorException : Exception
{
    public CriticalCalculationErrorException(decimal accountId) : base($"A critical calculation error occurred while processing your request. Account id:{accountId}")
    {
    }

    public CriticalCalculationErrorException(string message) : base(message)
    {
    }

    public CriticalCalculationErrorException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}