namespace CarAuctionManagementSystem.API.ExceptionHandler.Models;

public class ResponseModel
{
    public object? Message { get; set; }
    public int StatusCode { get; set; }
    public bool Success { get; set; }
}