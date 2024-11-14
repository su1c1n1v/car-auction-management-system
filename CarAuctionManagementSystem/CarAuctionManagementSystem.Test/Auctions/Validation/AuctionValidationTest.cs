using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Validation;

namespace CarAuctionManagementSystem.Test.Auctions.Validation;

public class AuctionValidationTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AuctionValidation_ShouldReturnError(string? vehicleId)
    {
        // Arrange
        var auction = new CreateAuctionDto
        {
            VehicleId = vehicleId!
        };
        
        var validator = new AuctionValidation();

        // Act
        var result = validator.Validate(auction);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void AuctionValidation_ShouldReturnSuccess()
    {
        // Arrange
        var auction = new CreateAuctionDto
        {
            VehicleId = "vehicleId"
        };
        
        var validator = new AuctionValidation();

        // Act
        var result = validator.Validate(auction);

        // Assert
        Assert.True(result.IsValid);
    }
}