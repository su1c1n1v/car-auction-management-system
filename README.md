# Car Auction Management System

This project is designed to create a vehicle auction management system. The goal is to develop a .NET API that allows users to place bids and create auctions for vehicles in a company inventory. The system supports four vehicle types (SUV, Sedan, Hatchback, and Truck), each with unique properties suited to their type.

## Code Structure

The API is built as a minimal .NET 8 API, organized by feature. Each feature has its own folder, unlike the typical structure in .NET where folders are divided by type (e.g., controllers, models, services). To group and manage endpoints by feature, the `Carter` library is used. The API is accessible and testable via Swagger. Unit tests are implemented with xUnit and Moq for easy mocking of dependencies. Logging is set up with Serilog, and message formats and severity levels are configured in `appsettings.json` to simplify environment-specific configurations.

Each feature has its own `ServiceCollection` extension to handle dependency injection in a centralized configuration file.

A global exception handler captures exceptions and returns a structured error response:

**Example Response:**

```json
{
  "message": "Not found",
  "statusCode": 404,
  "success": false
}
```

Input validation is managed using FluentValidation, a robust framework for enforcing validation rules.

## Business Rules

### Vehicle Management

Vehicles can be added and retrieved based on various attributes, including manufacturer, model, type, and year. Each vehicle can be of one of four types:

- **Truck**: Includes `Load Capacity`
- **Hatchback**: Includes `Number of Doors`
- **Sedan**: Includes `Number of Doors`
- **SUV**: Includes `Number of Seats`

**Vehicle Example:**

```json
{
  "type": {
    "id": 0,
    "name": "Hatchback"
  },
  "numberDoors": 3,
  "id": "44e60c9a-6374-4a5a-b1cd-496436c999c6",
  "manufacturer": "Toyota",
  "model": "Corolla",
  "year": 2019,
  "startingBid": 8484,
  "createdAt": "2024-11-13T19:30:29.4708184Z"
}
```

#### Creating a Vehicle

When creating a vehicle, the required properties depend on the vehicle type. For instance, trucks require `Load Capacity`, while hatchbacks require `Number of Doors`.

**Vehicle Creation Payload Example:**

```json
{
  "manufacturer": "string",
  "model": "string",
  "type": 0,
  "year": 0,
  "startingBid": 0,
  "numberSeats": 0,
  "loadCapacity": 0,
  "numberDoors": 0
}
```

### Auction Management

A vehicle can have multiple auctions; however, only one auction can be in an `Active` or `Pending` state at a time. Previous auctions must not end with the vehicle in a `Sold` state. All bids are stored, and the highest bid at auction end wins the vehicle.

An auction can have four statuses:

- **Pending**: Awaiting start
- **Active**: Open for bids
- **Sold**: Completed and sold
- **Cancelled**: Closed without successful bids

**Auction Example:**

```json
{
  "id": "b5b94cf1-9d71-4bb2-a534-50cecc005e10",
  "start": null,
  "end": null,
  "vehicle": {
    "type": {
      "id": 0,
      "name": "Hatchback"
    },
    "numberDoors": 3,
    "id": "44e60c9a-6374-4a5a-b1cd-496436c999c6",
    "manufacturer": "Toyota",
    "model": "Corolla",
    "year": 2019,
    "startingBid": 8484,
    "createdAt": "2024-11-13T19:30:29.4708184Z"
  },
  "bids": [],
  "status": {
    "id": 0,
    "name": "Pending"
  },
  "createdAt": "2024-11-13T19:30:46.9259224Z"
}
```

#### Creating an Auction

New auctions start with a `Pending` status.

**Auction Creation Payload Example:**

```json
{
  "vehicleId": "string"
}
```

If the vehicle is already part of an auction with a status of `Active`, `Pending`, or `Sold`, a `BadRequest` response is returned.

#### Starting an Auction

To start an auction, send a request to `/auctions/{id}/start`. The auction must be in the `Pending` state. If the auction is not `Pending`, a `BadRequest` exception is returned.

#### Ending an Auction

To close an auction, request `/auctions/{id}/finish`. The auction must be in the `Active` state. If the auction has no bids when it finishes, its status changes to `Cancelled`; otherwise, it concludes as `Sold` with the highest bid. If the auction is not `Active`, a `BadRequest` exception is returned.

#### Placing a Bid

A bid must exceed the highest current bid (or the `StartingBid` if there are no bids). If the bid amount is lower than the current highest bid, a `BadRequest` exception is returned.

**Bid Payload Example:**

```json
{
  "username": "string",
  "amount": 0
}
```

## Technologies Used

- **xUnit**: For unit testing
- **Moq**: For dependency injection mocking
- **Carter**: To organize and group endpoints by feature
- **Serilog**: For logging and message configuration
- **FluentValidation**: For input validation
