namespace Pos.Api.BusinessStaff.Models.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
