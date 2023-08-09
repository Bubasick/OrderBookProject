using OrderBook.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrderBook.Api;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case NotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case RequestExceedsMarketException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case BalanceTooLowException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case EntityShouldBeUniqueException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;


                case CriticalCalculationErrorException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}