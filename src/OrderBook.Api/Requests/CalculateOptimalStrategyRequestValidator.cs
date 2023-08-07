using FluentValidation;
using OrderBook.Api.Requests;
using OrderBook.Domain;

public class CalculateOptimalStrategyRequestValidator : AbstractValidator<CalculateOptimalStrategyRequest>
{


    public CalculateOptimalStrategyRequestValidator()
    {
        RuleFor(request => request.Accounts)
            .Must(accounts => accounts.Distinct(new AccountComparer()).Count() == accounts.Count())
            .WithMessage("Accounts should have unique id's.");
    }
}