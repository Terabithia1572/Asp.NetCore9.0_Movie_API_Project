using System;

namespace MovieApi.DTOs.DTOs.UserDTOs
{
    public class ResultUserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public string FullName => $"{Name} {Surname}".Trim();
    }
}
