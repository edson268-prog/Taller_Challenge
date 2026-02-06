namespace Taller_Challenge_Backend.Infrastructure.Identity
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 5;
    }
}
