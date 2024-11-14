using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Interfaces;
using Carter;
using FluentValidation;

namespace CarAuctionManagementSystem.API.Auctions;

public class AuctionModule : CarterModule
{
    private readonly ILogger<AuctionModule> _logger;

    public AuctionModule(ILogger<AuctionModule> logger) : base("auctions")
    {
        _logger = logger;

        WithDescription("An API resource for Auctions");
        WithName("Auctions API ");
        WithTags("Auctions");
        WithDisplayName("Auctions");
        WithGroupName("v1");
        IncludeInOpenApi();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (IAuctionService auctionService) =>
            {
                _logger.LogInformation("Getting all auctions");

                var result = await auctionService.GetAllAsync();

                _logger.LogInformation("Returning all auctions");

                return Results.Ok(result);
            })
            .WithName("GetAllAuctions")
            .WithDescription("Get all auctions");

        app.MapGet("/{id}", async (IAuctionService auctionService, string id) =>
            {
                _logger.LogInformation("Getting auction by id");

                var result = await auctionService.GetByIdAsync(id);

                _logger.LogInformation("Returning auction by id");

                return Results.Ok(result);
            })
            .WithName("GetAuctionById")
            .WithDescription("Get auction by id");

        app.MapPost("/",
                async (IAuctionService auctionService, IValidator<CreateAuctionDto> validator,
                    CreateAuctionDto createAuctionDto) =>
                {
                    _logger.LogInformation("Adding new auction");

                    await validator.ValidateAndThrowAsync(createAuctionDto);

                    var result = await auctionService.AddAsync(createAuctionDto);

                    _logger.LogInformation("New auction added");

                    return Results.Created($"/auctions/{result.Id}", result);
                })
            .WithName("CreateAuction")
            .WithDescription("Create a new auction");

        app.MapPut("/{id}/start", async (IAuctionService auctionService, string id) =>
            {
                _logger.LogInformation("Starting auction");

                await auctionService.StartAsync(id);

                _logger.LogInformation("Auction started");

                return Results.NoContent();
            })
            .WithName("StartAuction")
            .WithDescription("Start auction");

        app.MapPut("/{id}/finish", async (IAuctionService auctionService, string id) =>
            {
                _logger.LogInformation("Finishing auction");

                await auctionService.FinishAsync(id);

                _logger.LogInformation("Auction finished");

                return Results.NoContent();
            })
            .WithName("FinishAuction")
            .WithDescription("Finish auction");

        app.MapPost("/{id}/bid", async (
                IAuctionService auctionService,
                IValidator<CreateBidDto> validator,
                string id,
                CreateBidDto createBidDto
            ) =>
            {
                _logger.LogInformation("Finishing auction");

                await validator.ValidateAndThrowAsync(createBidDto);

                await auctionService.OfferBidAsync(id, createBidDto);

                _logger.LogInformation("Auction finished");

                return Results.Created($"/auctions/{id}", createBidDto);
            })
            .WithName("OfferBid")
            .WithDescription("Offer bid");
    }
}