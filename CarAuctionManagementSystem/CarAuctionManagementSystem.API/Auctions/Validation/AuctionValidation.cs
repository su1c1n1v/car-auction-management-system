using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Auctions.Validation;

public class AuctionValidation : AbstractValidator<CreateAuctionDto>
{
    public AuctionValidation()
    {
        RuleFor(vehicle => vehicle.VehicleId)
            .NotNull()
            .MinimumLength(3);
    }
}