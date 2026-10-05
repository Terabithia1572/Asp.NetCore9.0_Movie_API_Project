namespace MovieApi.DTOs.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
