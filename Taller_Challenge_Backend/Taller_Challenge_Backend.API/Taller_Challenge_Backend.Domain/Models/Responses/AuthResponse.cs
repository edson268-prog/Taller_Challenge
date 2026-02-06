using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Domain.Models.Responses
{
    public record AuthResponse(
        string Token,
        DateTime ExpiresAt,
        string TokenType,
        int UserId,
        string Username,
        string Email,
        string Role
    );
}
