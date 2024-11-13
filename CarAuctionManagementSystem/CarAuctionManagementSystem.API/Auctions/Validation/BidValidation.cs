using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Auctions.Validation;

public class BidValidation : AbstractValidator<CreateBidDto>
{
    public BidValidation()
    {
        RuleFor(vehicle => vehicle.Username)
            .NotNull()
            .MinimumLength(5);

        RuleFor(vehicle => vehicle.Amount)
            .NotNull()
            .InclusiveBetween(1, double.MaxValue);
    }
}