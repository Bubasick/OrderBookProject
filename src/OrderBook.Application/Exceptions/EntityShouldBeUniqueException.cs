using System.Globalization;

namespace OrderBook.Application.Exceptions;

public class EntityShouldBeUniqueException : Exception
{
    public EntityShouldBeUniqueException(string entityName) : base($"{entityName}s should be unique")
    {
    }

    public EntityShouldBeUniqueException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    public override string ToString()
    {
        // Return anything you need here
        return Message;
    }
}