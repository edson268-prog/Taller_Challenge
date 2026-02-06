using System.ComponentModel.DataAnnotations;
using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; private set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; private set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Email { get; private set; } = string.Empty;

        [Required]
        public Role RoleId { get; private set; } = Role.Visitor;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public static User Create(string username, string passwordHash, string email, Role role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            var user = new User
            {
                Username = username.Trim(),
                PasswordHash = passwordHash,
                Email = email.Trim(),
                RoleId = role,
                CreatedAt = DateTime.UtcNow
            };

            return user;
        }
    }
}
