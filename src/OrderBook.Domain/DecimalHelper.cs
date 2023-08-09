namespace OrderBook.Domain;

public static class DecimalHelper
{
    public static decimal Round(this decimal value, int precision, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
    {
        return decimal.Round(value, precision, midpointRounding);
    }
}