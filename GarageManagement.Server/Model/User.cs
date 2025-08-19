namespace GarageManagement.Server.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin" or "User"
        // Password reset support
        public string? ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiresUtc { get; set; }
    }

}
