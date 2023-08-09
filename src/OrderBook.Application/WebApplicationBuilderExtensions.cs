using Microsoft.Extensions.DependencyInjection;
using OrderBook.Application.Interfaces;
using OrderBook.Application.Services;

namespace OrderBook.Application;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICalculationService, CalculationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}