using CarAuctionManagementSystem.API.ExceptionHandler.Models;
using FluentValidation;

namespace CarAuctionManagementSystem.API.ExceptionHandler;

public class ExceptionHandleMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (BadHttpRequestException ex)
        {
            await HandleBadHttpRequestException(ex, httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(ex, httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(ex, httpContext);
        }
    }

    private async Task HandleBadHttpRequestException(BadHttpRequestException ex, HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ResponseModel
        {
            Message = ex.Message,
            StatusCode = StatusCodes.Status400BadRequest,
            Success = false
        });
    }

    private async Task HandleValidationException(ValidationException ex, HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ResponseModel
        {
            Message = ex.Errors,
            StatusCode = StatusCodes.Status400BadRequest,
            Success = false
        });
    }

    private async Task HandleException(Exception ex, HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new ResponseModel
        {
            Message = ex.Message,
            StatusCode = StatusCodes.Status500InternalServerError,
            Success = false
        });
    }
}