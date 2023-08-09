using Moq;
using OrderBook.Application.Services;
using OrderBook.Domain.Entities;

namespace OrderBook.Tests
{
    public class CalculationServiceTests
    {
        private readonly Mock<OrderService> _orderServiceMock;
        private readonly Mock<AccountService> _accountServiceMock;
        private readonly CalculationService _service;

        public CalculationServiceTests()
        {
            _orderServiceMock = new Mock<OrderService>(null);
            _orderServiceMock.Setup(x=> x.GetOrdersForSells(It.IsAny<IEnumerable<decimal>>())).Returns(TestDataHelper.GetFakeOrderForSellList());
            _orderServiceMock.Setup(x => x.GetOrdersForBuys(It.IsAny<IEnumerable<decimal>>())).Returns(TestDataHelper.GetFakeOrderForBuyList());

            _accountServiceMock = new Mock<AccountService>();
            _accountServiceMock.Setup(x => x.CheckIfBtcBalanceEmpty(It.IsAny<List<Account>>()));
            _accountServiceMock.Setup(x => x.CheckIfEuroBalanceEmpty(It.IsAny<List<Account>>()));
            _accountServiceMock.Setup(x => x
                    .ValidateAndFilterAccounts(It.IsAny<List<Account>>(), It.IsAny<OperationType>(), It.IsAny<decimal>()))
                    .Returns((List<Account> accounts, OperationType operation, decimal value) => accounts);

            _service = new CalculationService(_orderServiceMock.Object, _accountServiceMock.Object);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenBuy_ShouldReturn_CalculatedData()
        {
            var btcAmount = 0.1m;
            var operation = OperationType.Buy;

            var accounts = new List<Account>()
            {
                new Account(1, 10, 200)
            };

            var expected = new Order()
            {
                Id = 1,
                Price = 2000,
                Type = OperationType.Buy
            };

            //Act
            var actual = _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.Price, actual[0].Price);
            Assert.Equal(expected.Type, actual[0].Type);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenSell_ShouldReturn_CalculatedData()
        {
            var btcAmount = 0.1m;
            var operation = OperationType.Sell;
            var accounts = new List<Account>()
            {
                new Account(1, 10, 200)
            };

            var expected = new Order()
            {
                Id = 1,
                Price = 2002,
                Type = OperationType.Sell
            };

            //Act
            var actual = _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.Price, actual[0].Price);
            Assert.Equal(expected.Type, actual[0].Type);
        }
    }
}