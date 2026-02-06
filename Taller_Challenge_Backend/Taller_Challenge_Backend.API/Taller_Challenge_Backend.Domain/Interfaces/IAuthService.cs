using Taller_Challenge_Backend.Domain.Models.Requests;
using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}
