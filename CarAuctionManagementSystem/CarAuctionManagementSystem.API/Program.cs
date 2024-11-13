using CarAuctionManagementSystem.API.Auctions;
using CarAuctionManagementSystem.API.ExceptionHandler;
using CarAuctionManagementSystem.API.Logging;
using CarAuctionManagementSystem.API.Vehicles;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddVehicleServices();
builder.Services.AddAuctionServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddExceptionHandleMiddleware();

app.MapCarter();
app.Run();
