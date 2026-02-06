namespace Taller_Challenge_Backend.Domain.Models.Requests
{
    public record AuthRequest(
        string Username,
        string Password
    );
}
