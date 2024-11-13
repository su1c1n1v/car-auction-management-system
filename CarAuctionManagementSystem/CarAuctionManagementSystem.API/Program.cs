using CarAuctionManagementSystem.API.Logging;
using CarAuctionManagementSystem.API.Vehicles;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddVehicleServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.Run();
