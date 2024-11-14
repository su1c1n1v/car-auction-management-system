using CarAuctionManagementSystem.API.Auctions.DataTransferObject;
using CarAuctionManagementSystem.API.Auctions.Validation;

namespace CarAuctionManagementSystem.Test.Auctions.Validation;

public class BidValidationTest
{
    [Theory]
    // Test for username
    [InlineData(null, 200.5)]
    [InlineData("", 1000.4)]
    // Test for amount
    [InlineData("username", -1.5)]
    public void BidValidation_ShouldReturnError(string? username, double? amount)
    {
        // Arrange
        var bid = new CreateBidDto
        {
            Username = username!,
            Amount = (double)amount!
        };
        var validator = new BidValidation();

        // Act
        var result = validator.Validate(bid);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void BidValidation_ShouldReturnSuccess()
    {
        // Arrange
        var bid = new CreateBidDto
        {
            Username = "username",
            Amount = 200.5
        };
        var validator = new BidValidation();

        // Act
        var result = validator.Validate(bid);

        // Assert
        Assert.True(result.IsValid);
    }
}