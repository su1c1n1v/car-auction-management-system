using Carter;

namespace CarAuctionManagementSystem.API.Vehicles;

public class VehicleModule(ILogger<VehicleModule> logger) : CarterModule("/api/vehicles")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
    }
}