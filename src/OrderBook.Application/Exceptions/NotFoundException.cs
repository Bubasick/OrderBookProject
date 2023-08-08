using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName) : base($"No {entityName}s that suits the request are found")
    {
    }

    public NotFoundException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}