using Microsoft.AspNetCore.Http;
using System.Data.Entity.Infrastructure;
using Microsoft.Extensions.Logging;
using PeterParker.Infrastructure;
using PeterParker.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
//using ExceptionHandling.Models.Responses;

namespace ExceptionHandling.CustomMiddlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse
        {
            Success = false
        };
        switch (exception)
        {
            case NotFoundException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = ex.Message;
                break;

            case MissingParametersException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                break;

            case IncorrectLoginInfoException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                break;

            case DuplicateObjectException ex:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                errorResponse.Message = ex.Message;
                break; 

            case ParkingSpaceTakenException ex:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                errorResponse.Message = ex.Message;
                break;

            case InvalidRefreshToken ex:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = ex.Message;
                break;

            case SqlException ex:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;

            case DbUpdateException ex:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An Error occured while accessing the database.";
                break;

            case InternalServerErrorException ex:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;

            case ApplicationException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                break;

            default:

                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Internal server error!";
                break;
        }

        _logger.LogError(exception.Message);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}