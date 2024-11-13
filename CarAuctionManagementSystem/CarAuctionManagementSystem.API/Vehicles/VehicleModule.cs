using CarAuctionManagementSystem.API.Vehicles.DataTransferObject;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using Carter;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Vehicles;

public class VehicleModule : CarterModule
{
    private readonly ILogger<VehicleModule> _logger;

    public VehicleModule(ILogger<VehicleModule> logger) : base("vehicles")
    {
        _logger = logger;

        WithDescription("An API resource for Vehicles");
        WithName("Vehicles API ");
        WithTags("Vehicles");
        WithDisplayName("Vehicle");
        WithGroupName("v1");
        IncludeInOpenApi();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (IVehicleService vehicleService) =>
            {
                _logger.LogInformation("Getting all vehicles");

                var result = await vehicleService.GetAllAsync();

                _logger.LogInformation("Returning all vehicles");

                return Results.Ok(result);
            })
            .WithName("GetAllVehicles")
            .WithDescription("Get all vehicles");

        app.MapGet("/{id}", async (IVehicleService vehicleService, string id) =>
            {
                _logger.LogInformation("Getting vehicle by id");

                var result = await vehicleService.GetByIdAsync(id);

                _logger.LogInformation("Returning vehicle by id");

                return Results.Ok(result);
            })
            .WithName("GetVehicleById")
            .WithDescription("Get vehicle by id");

        app.MapGet("/search", async (IVehicleService vehicleService, string? manufacturer, string? model, int? year) =>
            {
                _logger.LogInformation("Searching vehicles");

                var result = await vehicleService.SearchAsync(manufacturer, model, year);

                _logger.LogInformation("Returning vehicles");

                return Results.Ok(result);
            })
            .WithName("SearchVehicles")
            .WithDescription("Search vehicles");
        
        app.MapPost("/", async (IVehicleService vehicleService, IValidator<CreateVehicleDto> validator, CreateVehicleDto vehicle) =>
            {
                _logger.LogInformation("Creating vehicle");
                
                await validator.ValidateAndThrowAsync(vehicle);

                var result = await vehicleService.AddAsync(vehicle);

                _logger.LogInformation("Returning created vehicle");

                return Results.Ok(result);
            })
            .WithName("CreateVehicle")
            .WithDescription("Create a vehicle");
    }
}