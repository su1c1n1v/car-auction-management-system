using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Models;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Vehicles.Validation;

public class VehicleValidation : AbstractValidator<CreateVehicleDto>
{
    public VehicleValidation()
    {
        RuleFor(vehicle => vehicle.Manufacturer)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(vehicle => vehicle.Model)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(vehicle => vehicle.Type)
            .NotNull()
            .IsInEnum();

        RuleFor(vehicle => vehicle.Year)
            .NotNull()
            .InclusiveBetween(1960, DateTime.Now.Year);

        RuleFor(vehicle => vehicle.StartingBid)
            .NotNull()
            .InclusiveBetween(1, int.MaxValue);

        RuleFor(vehicle => vehicle.LoadCapacity)
            .NotNull()
            .InclusiveBetween(1, 74000)
            .When(vehicle => vehicle.Type == VehicleEnumType.Truck);

        RuleFor(vehicle => vehicle.NumberSeats)
            .NotNull()
            .InclusiveBetween(1, 40)
            .When(vehicle => vehicle.Type == VehicleEnumType.SUV);

        RuleFor(vehicle => vehicle.NumberDoors)
            .NotNull()
            .InclusiveBetween(1, 5)
            .When(vehicle => vehicle.Type is VehicleEnumType.Hatchback or VehicleEnumType.Sedan);
    }
}