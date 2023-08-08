using OrderBook.Application;
using OrderBook.Application.Exceptions;
using OrderBook.Domain;

namespace OrderBook.Tests
{
    public class OrderBookServiceTests
    {
        private OrderBookService _service;

        private List<Order> orders;

        public OrderBookServiceTests()
        {
            _service = new OrderBookService();
             orders = new List<Order>()
            {
                new Order()
                {
                    Id = (decimal)1,
                    Amount = (decimal)0.1,
                    Price = 2002,
                    Type = OperationType.Buy
                },
                new Order()
                {
                    Id = (decimal)2,
                    Amount = (decimal)0.2,
                    Price = 2000,
                    Type = OperationType.Buy
                },
                new Order()
                {
                    Id = (decimal)1,
                    Amount = (decimal)0.1,
                    Price = 2000,
                    Type = OperationType.Sell
                },
                new Order()
                {
                    Id = (decimal)2,
                    Amount = (decimal)0.2,
                    Price = 2002,
                    Type = OperationType.Sell
                }
            };
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenBuy_ShouldReturn_CalculatedData()
        {
            decimal btcAmount = (decimal)0.1;
            var operation = OperationType.Buy;
            List<Account> accounts = new List<Account>()
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
            var actual =  _service.CalculateOptimalStrategy(orders,accounts,operation,btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.Price, actual[0].Price);
            Assert.Equal(expected.Type, actual[0].Type);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenNoSuitableOrders_ShouldThrow_NotFoundException()
        {
            decimal btcAmount = (decimal)0.1;
            var operation = OperationType.Buy;
            List<Account> accounts = new List<Account>()
            {
                new Account(3, 10, 200)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(orders, accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<NotFoundException>(actual);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenNotEnoughBalanceOnAccounts_ShouldThrow_BalanceTooLowException()
        {
            decimal btcAmount = (decimal)0.1;
            var operation = OperationType.Buy;
            List<Account> accounts = new List<Account>()
            {
                new Account(1, 0, 0)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(orders, accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<BalanceTooLowException>(actual);
        }

        [Fact]
        public void CalculateOptimalStrategy_WhenAmountExceedsAvailableOnMarket_ShouldThrow_RequestExceedsMarketException()
        {
            decimal btcAmount = (decimal)100;
            var operation = OperationType.Buy;
            List<Account> accounts = new List<Account>()
            {
                new Account(1, 0, 100000000)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(orders, accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<RequestExceedsMarketException>(actual);
        }
        [Fact]
        public void CalculateOptimalStrategy_WhenAccountIdsAreNotUnique_ShouldThrow_EntityShouldBeUniqueException()
        {
            decimal btcAmount = (decimal)100;
            var operation = OperationType.Buy;
            List<Account> accounts = new List<Account>()
            {
                new Account(1, 0, 100000000),
                new Account(1, 0, 100000000)
            };

            //Act
            var actual = () => _service.CalculateOptimalStrategy(orders, accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Throws<EntityShouldBeUniqueException>(actual);
        }


        [Fact]
        public void CalculateOptimalStrategy_WhenSell_ShouldReturn_CalculatedData()
        {
            decimal btcAmount = (decimal)0.1;
            var operation = OperationType.Sell;
            List<Account> accounts = new List<Account>()
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
            var actual = _service.CalculateOptimalStrategy(orders, accounts, operation, btcAmount);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual[0].Id);
            Assert.Equal(expected.Price, actual[0].Price);
            Assert.Equal(expected.Type, actual[0].Type);
        }
    }
}