using Microsoft.AspNetCore.Mvc;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Requests;

namespace Taller_Challenge_Backend.API.Auth.Commands
{
    public class AuthCommand
    {
        public static async Task<IResult> ExecuteQuery([FromBody] AuthRequest request, [FromServices] IAuthService authService)
        {
            if (request == null)
                return Results.BadRequest("Request body is required");

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return Results.BadRequest("Username and password are required");

            if (request.Username.Length > 20 || request.Password.Length > 20)
                return Results.BadRequest("Username and password must not exceed 20 characters");

            var response = await authService.AuthenticateAsync(request);
            return Results.Ok(response);
        }
    }
}
