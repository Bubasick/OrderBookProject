using Moq;
using OrderBook.Application;
using OrderBook.Application.Exceptions;
using OrderBook.Domain.Entities;
using OrderBook.Infrastructure;

namespace OrderBook.Tests
{
    public class OrderBookServiceTests
    {
        private readonly Mock<DataReaderService> _dataReaderMock;
        private readonly OrderBookService _service;

        public OrderBookServiceTests()
        {
            _dataReaderMock = new Mock<DataReaderService>();

            _dataReaderMock.Setup(x => x.GetOrders()).Returns(TestDataHelper.GetFakeOrderList());

            _service = new OrderBookService(_dataReaderMock.Object);
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
        public void CalculateOptimalStrategy_WhenNoSuitableOrders_ShouldThrow_NotFoundException()
        {
            var btcAmount = 0.1m;
            var operation = OperationType.Buy;
            var accounts = new List<Account>()
            {
                new Account(3, 10, 200)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<NotFoundException>(actual);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenNotEnoughBalanceOnAccounts_ShouldThrow_BalanceTooLowException()
        {
            var btcAmount = 0.1m;
            var operation = OperationType.Buy;
            var accounts = new List<Account>()
            {
                new Account(1, 0, 0)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<BalanceTooLowException>(actual);
        }

        [Fact]
        public void
            CalculateOptimalStrategy_WhenAmountExceedsAvailableOnMarket_ShouldThrow_RequestExceedsMarketException()
        {
            var btcAmount = 100m;
            var operation = OperationType.Buy;
            var accounts = new List<Account>()
            {
                new Account(1, 0, 100000000)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<RequestExceedsMarketException>(actual);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenAccountIdsAreNotUnique_ShouldThrow_EntityShouldBeUniqueException()
        {
            var btcAmount = 100m;
            var operation = OperationType.Buy;
            var accounts = new List<Account>()
            {
                new Account(1, 0, 100000000),
                new Account(1, 0, 100000000)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<EntityShouldBeUniqueException>(actual);
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