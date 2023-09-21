using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PeterParker.Data.Models;
using PeterParker.DTOs;
using PeterParker.Infrastructure.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class UserValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is a POST request targeting the User endpoint
        if (context.Request.Method == HttpMethods.Post &&
            (context.Request.Path.StartsWithSegments("/api/User/Register")||
            context.Request.Path.StartsWithSegments("/api/User/Update"))) // Update the path as needed
        {
            // Read the request body
            try
            {
                string requestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var user = JsonConvert.DeserializeObject<UserRegisterDTO>(requestBody);

                IsValidUser(user);
            }
            catch (JsonReaderException ex)
            {
                // Handle JSON parsing errors
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Invalid JSON",
                    detail = "The request body does not contain valid JSON.",
                };
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
                return; // End the middleware pipeline
            }
        }

        await _next(context);
    }

    private void IsValidUser(UserRegisterDTO user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName) ||
            string.IsNullOrWhiteSpace(user.LastName))
        {
            throw new BadUserDataException("No field should be empty.");
        }

        if (user.FirstName.Length > 12 || user.FirstName.Length < 2)
        {
            throw new BadUserDataException("First name cannot be longer than 12 characters or shorter than 2.");
        }

        if (user.LastName.Length > 20 || user.LastName.Length < 2)
        {
            throw new BadUserDataException("Last name cannot be longer than 20 characters or shorter than 2.");
        }
    }
}
