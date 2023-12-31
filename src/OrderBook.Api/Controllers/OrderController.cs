using Microsoft.AspNetCore.Mvc;
using OrderBook.Application.Interfaces;
using OrderBook.Domain.Entities;

namespace OrderBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ICalculationService _orderBookService;

        public OrderController(ICalculationService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        [HttpPost("CalculateOptimalStrategy")]
        public IEnumerable<Order> CalculateOptimalStrategy([FromBody] List<Account> accounts, OperationType operation, decimal btcAmount)
        {
            return _orderBookService.CalculateOptimalStrategy(accounts, operation, btcAmount);
        }
    }
}