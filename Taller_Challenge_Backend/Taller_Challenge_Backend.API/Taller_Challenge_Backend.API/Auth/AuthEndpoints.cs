using Asp.Versioning.Builder;
using Taller_Challenge_Backend.API.Auth.Commands;
using Taller_Challenge_Backend.API.Common;
using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.API.Auth
{
    public class AuthEndpoints : IEndpoints
    {
        public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder, ApiVersionSet versions)
        {
            var group = builder.MapGroup("/api/v1/auth")
                .WithTags("Auth")
                .WithApiVersionSet(versions)
                .MapToApiVersion(1);

            group.MapPost("/", AuthCommand.ExecuteQuery)
                .Produces<AuthResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithName("Authentication")
                .WithSummary("Authenticate user")
                .WithDescription("Verify credentials and generate JWT token for session");

            return builder;
        }
    }
}
