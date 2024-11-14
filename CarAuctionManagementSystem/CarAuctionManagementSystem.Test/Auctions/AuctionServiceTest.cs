using CarAuctionManagementSystem.API.Auctions;
using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Interfaces;
using CarAuctionManagementSystem.API.Auctions.Models;
using CarAuctionManagementSystem.API.Vehicles.Interfaces;
using CarAuctionManagementSystem.API.Vehicles.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CarAuctionManagementSystem.Test.Auctions;

public class AuctionServiceTest
{
    [Fact]
    public async void GetAllAsync_ShouldReturnAllAuctions()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auctions = new List<Auction>
        {
            new(new Truck { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 }),
            new(new SUV { Manufacturer = "Toyota", Model = "Camry", Year = 2015 }),
            new(new Hatchback { Manufacturer = "Honda", Model = "Civic", Year = 2018 })
        };

        auctionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(auctions);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(auctions, result);
    }

    [Fact]
    public async void GetByIdAsync_ShouldReturnAuction()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Truck { Manufacturer = "Toyota", Model = "Corolla", Year = 2010 });

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);

        // Act
        var result = await service.GetByIdAsync("1");

        // Assert
        Assert.Equal(auction, result);
    }

    [Fact]
    public async void GetByIdAsync_ShouldThrowException_WhenAuctionNotFound()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync((Auction)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync("1"));
    }

    [Fact]
    public async void AddAsync_ShouldAddNewAuction()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var vehicle = new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        };

        var auction = new CreateAuctionDto
        {
            VehicleId = "1"
        };

        vehicleRepository.Setup(x => x.GetByIdAsync(auction.VehicleId)).ReturnsAsync(vehicle);
        auctionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Auction>());
        auctionRepository.Setup(x => x.AddAsync(It.IsAny<Auction>())).ReturnsAsync(new Auction(vehicle));

        // Act
        var result = await service.AddAsync(auction);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicle, result.Vehicle);
    }

    [Fact]
    public async void AddAsync_ShouldThrowException_WhenVehicleNotFound()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new CreateAuctionDto
        {
            VehicleId = "1"
        };

        vehicleRepository.Setup(x => x.GetByIdAsync(auction.VehicleId)).ReturnsAsync((Vehicle)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.AddAsync(auction));
    }

    [Theory]
    [InlineData(AuctionStatusEnum.Active)]
    [InlineData(AuctionStatusEnum.Pending)]
    [InlineData(AuctionStatusEnum.Sold)]
    public async void AddAsync_ShouldThrowException_WhenAuctionAlreadyExists(AuctionStatusEnum status)
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var vehicle = new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        };

        var auction = new CreateAuctionDto
        {
            VehicleId = "1"
        };

        var existingAuction = new List<Auction>
        {
            new(vehicle)
            {
                Status = new AuctionStatus(status)
            }
        };

        vehicleRepository.Setup(x => x.GetByIdAsync(auction.VehicleId)).ReturnsAsync(vehicle);
        auctionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(existingAuction);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.AddAsync(auction));
    }

    [Fact]
    public async void StartAsync_ShouldStartAuction()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);
        auctionRepository.Setup(x => x.UpdateAsync(auction)).ReturnsAsync(auction);

        // Act
        await service.StartAsync("1");

        // Assert
        Assert.Equal(AuctionStatusEnum.Active, auction.Status.Id);
    }

    [Theory]
    [InlineData(AuctionStatusEnum.Active)]
    [InlineData(AuctionStatusEnum.Cancelled)]
    [InlineData(AuctionStatusEnum.Sold)]
    public async void StartAsync_ShouldThrowException_WhenAuctionAlreadyStarted(AuctionStatusEnum status)
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(status);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.StartAsync("1"));
    }

    [Fact]
    public async void FinishAsync_ShouldEndAuction()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(AuctionStatusEnum.Active);
        auction.Bids =
        [
            new Bid
            {
                Amount = 10000
            }
        ];

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);
        auctionRepository.Setup(x => x.UpdateAsync(auction)).ReturnsAsync(auction);

        // Act
        await service.FinishAsync("1");

        // Assert
        Assert.Equal(AuctionStatusEnum.Sold, auction.Status.Id);
    }

    [Fact]
    public async void FinishAsync_ShouldEndAuction_WhenNoBids()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(AuctionStatusEnum.Active);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);
        auctionRepository.Setup(x => x.UpdateAsync(auction)).ReturnsAsync(auction);

        // Act
        await service.FinishAsync("1");

        // Assert
        Assert.Equal(AuctionStatusEnum.Cancelled, auction.Status.Id);
    }

    [Theory]
    [InlineData(AuctionStatusEnum.Pending)]
    [InlineData(AuctionStatusEnum.Sold)]
    [InlineData(AuctionStatusEnum.Cancelled)]
    public async void FinishAsync_ShouldThrowException_WhenAuctionNotActive(AuctionStatusEnum status)
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(status);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.FinishAsync("1"));
    }

    [Fact]
    public async void OfferBidAsync_ShouldAddBid()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(AuctionStatusEnum.Active);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);
        auctionRepository.Setup(x => x.UpdateAsync(auction)).ReturnsAsync(auction);

        // Act
        await service.OfferBidAsync("1", new CreateBidDto { Amount = 10000, Username = "user" });

        // Assert
        Assert.Single(auction.Bids);
    }
    
    [Theory]
    [InlineData(AuctionStatusEnum.Pending)]
    [InlineData(AuctionStatusEnum.Sold)]
    [InlineData(AuctionStatusEnum.Cancelled)]
    public async void OfferBidAsync_ShouldThrowException_WhenAuctionNotActive(AuctionStatusEnum status)
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(status);

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.OfferBidAsync("1", new CreateBidDto { Amount = 10000, Username = "user" }));
    }
    
    [Fact]
    public async void OfferBidAsync_ShouldThrowException_WhenBidAmountIsLessThanMaxBid()
    {
        // Arrange
        var auctionRepository = new Mock<IAuctionRepository>();
        var logger = new Mock<ILogger<AuctionService>>();
        var vehicleRepository = new Mock<IVehicleRepository>();
        var service = new AuctionService(auctionRepository.Object, logger.Object, vehicleRepository.Object);

        var auction = new Auction(new Hatchback
        {
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2010
        });

        auction.Status = new AuctionStatus(AuctionStatusEnum.Active);
        auction.Bids =
        [
            new Bid
            {
                Amount = 10000
            }
        ];

        auctionRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(auction);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(() => service.OfferBidAsync("1", new CreateBidDto { Amount = 5000, Username = "user" }));
    }
}