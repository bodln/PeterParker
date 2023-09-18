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
            string requestBody;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            // Deserialize the JSON request body to a User object
            var user = JsonConvert.DeserializeObject<UserRegisterDTO>(requestBody);

            IsValidUser(user);
        }

        await _next(context);
    }

    private void IsValidUser(UserRegisterDTO user)
    {
        if (!string.IsNullOrWhiteSpace(user.FirstName) &&
            !string.IsNullOrWhiteSpace(user.LastName))
        {
            throw new BadUserDataException("No field should be empty.");
        }

        if (!(user.FirstName.Length > 12 || user.FirstName.Length < 2))
        {
            throw new BadUserDataException("First name cannot be longer than 12 characters or shorter than 2.");
        }

        if (!(user.LastName.Length > 20 || user.LastName.Length < 2))
        {
            throw new BadUserDataException("Last name cannot be longer than 20 characters or shorter than 2.");
        }
    }
}
