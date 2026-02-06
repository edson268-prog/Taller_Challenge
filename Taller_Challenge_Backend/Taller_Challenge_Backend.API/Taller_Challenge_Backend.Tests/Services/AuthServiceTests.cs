using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;
using Taller_Challenge_Backend.Domain.Models.Requests;
using Taller_Challenge_Backend.Infrastructure.Data;
using Taller_Challenge_Backend.Infrastructure.Helpers;
using Taller_Challenge_Backend.Infrastructure.Identity;
using Taller_Challenge_Backend.Infrastructure.Services;

namespace Taller_Challenge_Backend.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly string _dbName;
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly JwtHelper _jwtHelper;

        public AuthServiceTests()
        {
            _dbName = $"AuthTestDb_{Guid.NewGuid()}";
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: _dbName)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new AppDbContext(_options);
            _context.Database.EnsureCreated();

            var jwtSettings = new JwtSettings
            {
                Key = "DrP65okSr9/wyVFOJc8Ab8mCF8BAsg==", // Another random test key
                ExpiryMinutes = 5
            };
            var optionsMock = new Mock<IOptions<JwtSettings>>();
            optionsMock.Setup(ap => ap.Value).Returns(jwtSettings);

            _jwtHelper = new JwtHelper(optionsMock.Object);
            _loggerMock = new Mock<ILogger<AuthService>>();

            _authService = new AuthService(_context, _jwtHelper, optionsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Should_ReturnToken_When_CredentialsAreValid()
        {
            var password = "test123";
            var user = User.Create("eibanez268", Base64Helper.Encode(password), "eibanez@exsquared.com", Role.Admin);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new AuthRequest ("eibanez268", password);

            var result = await _authService.AuthenticateAsync(request);

            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal("Bearer", result.TokenType);
        }

        [Fact]
        public async Task Should_ThrowUnauthorized_When_UserNotExist()
        {
            var request = new AuthRequest("luis", "test123");

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.AuthenticateAsync(request)
            );

            Assert.Equal("Invalid username or password", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowUnauthorized_When_PasswordIsWrong()
        {
            var password = "exsquaredadmin123";
            var user = User.Create("eibanez268", Base64Helper.Encode(password), "eibanez@exsquared.com", Role.Admin);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var request = new AuthRequest ("admin", "admin");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.AuthenticateAsync(request)
            );
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
