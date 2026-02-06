using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Domain.Models.Requests;
using Taller_Challenge_Backend.Domain.Models.Responses;
using Taller_Challenge_Backend.Infrastructure.Data;
using Taller_Challenge_Backend.Infrastructure.Helpers;
using Taller_Challenge_Backend.Infrastructure.Identity;

namespace Taller_Challenge_Backend.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, JwtHelper jwtHelper, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
        {
            var user = await _context.Users.
                FirstOrDefaultAsync(u => u.Username == request.Username
                && u.PasswordHash == Base64Helper.Encode(request.Password));

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var token = _jwtHelper.GenerateToken(user.Id, user.Username, user.Email, user.RoleId);

            _logger.LogInformation("Token generated successfully");

            return new AuthResponse(token, DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), "Bearer", user.Id, user.Username, user.Email, user.RoleId.ToString());
        }
    }
}
