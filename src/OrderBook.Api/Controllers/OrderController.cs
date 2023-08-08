using Microsoft.AspNetCore.Mvc;
using OrderBook.Api.Requests;
using OrderBook.Application;
using OrderBook.Domain.Entities;

namespace OrderBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBookService _service;
        public OrderController(IOrderBookService service)
        {
            _service = service;
        }

        [HttpPost]
        public IEnumerable<Order> CalculateOptimalStrategy([FromBody] CalculateOptimalStrategyRequest request, OperationType operation, decimal btcAmount)
        {
            return _service.CalculateOptimalStrategy(request.Orders, request.Accounts, operation, btcAmount);
        }
    }
}