using OrderBook.Application.Exceptions;
using OrderBook.Application.Services;
using OrderBook.Domain.Entities;

namespace OrderBook.Tests;

public class AccountServiceTests
{
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _service = new AccountService();
    }

    [Fact]
    public void CheckIfBtcBalanceEmpty_ShouldThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 0, 12)
        };

        var result = () => _service.CheckIfBtcBalanceEmpty(accounts);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<BalanceTooLowException>(result);
    }

    [Fact]
    public void CheckIfBtcBalanceEmpty_ShouldNotThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 1, 12)
        };

        var result = Record.Exception(() => _service.CheckIfBtcBalanceEmpty(accounts));

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void CheckIfEuroBalanceEmpty_ShouldThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 0, 0)
        };

        var result = () => _service.CheckIfBtcBalanceEmpty(accounts);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<BalanceTooLowException>(result);
    }

    [Fact]
    public void CheckIfEuroBalanceEmpty_ShouldNotThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 1, 12)
        };

        var result = Record.Exception(() => _service.CheckIfEuroBalanceEmpty(accounts));

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateAndFilterAccounts_WhenOperationTypeBuy_ShouldThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 1, 0)
        };

        var result = () => _service.ValidateAndFilterAccounts(accounts, OperationType.Buy, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<BalanceTooLowException>(result);
    }

    [Fact]
    public void ValidateAndFilterAccounts_WhenOperationTypeSell_ShouldThrow_BalanceTooLowException()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 1, 0)
        };

        var result = () => _service.ValidateAndFilterAccounts(accounts, OperationType.Sell, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Throws<BalanceTooLowException>(result);
    }

    [Fact]
    public void ValidateAndFilterAccounts_ShouldReturn_FilteredAccounts()
    {
        var accounts = new List<Account>()
        {
            new Account(1, 1, 12),
            new Account(2, 0, 0)
        };

        var result = _service.ValidateAndFilterAccounts(accounts, OperationType.Buy, 10);

        //Assert
        Assert.Equal(1, result[0].MetaExchangeId);
    }
}