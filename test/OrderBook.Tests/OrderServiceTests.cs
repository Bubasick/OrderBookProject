using Moq;
using OrderBook.Application.Exceptions;
using OrderBook.Application.Services;
using OrderBook.Domain.Entities;
using OrderBook.Infrastructure;

namespace OrderBook.Tests;

public class OrderServiceTests
{
    private readonly OrderService _service;
    private readonly Mock<DataReaderService> _dataReaderMock;

    public OrderServiceTests()
    {
        _dataReaderMock = new Mock<DataReaderService>();
        _dataReaderMock.Setup(x => x.GetOrders()).Returns(TestDataHelper.GetFakeOrderList());
        _service = new OrderService(_dataReaderMock.Object);
    }

    [Fact]
    public void GetOrdersForBuys_ShouldReturn_CorrectOrder()
    {
        var ids = new List<decimal>()
        {
            1m
        };

        var result = _service.GetOrdersForBuys(ids);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Equal(ids[0], result[0].Id);
        Assert.Equal(OperationType.Sell, result[0].Type);
    }

    [Fact]
    public void GetOrdersForBuys_ShouldThrow_NotFoundException()
    {
        var ids = new List<decimal>()
        {
            3m
        };

        var result = () => _service.GetOrdersForBuys(ids);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<NotFoundException>(result);
    }

    [Fact]
    public void GetOrdersForSells_ShouldReturn_CorrectOrder()
    {
        var ids = new List<decimal>()
        {
            1m
        };

        var result = _service.GetOrdersForSells(ids);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Equal(ids[0], result[0].Id);
        Assert.Equal(OperationType.Buy, result[0].Type);
    }

    [Fact]
    public void GetOrdersForSells_ShouldThrow_NotFoundException()
    {
        var ids = new List<decimal>()
        {
            3m
        };

        var result = () => _service.GetOrdersForSells(ids);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<NotFoundException>(result);
    }
}