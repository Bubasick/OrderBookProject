namespace OrderBook.Domain;

public static class DecimalHelper
{
    public static decimal Round(this decimal value, int precision)
    {
        return value.Round(precision);
    }
}