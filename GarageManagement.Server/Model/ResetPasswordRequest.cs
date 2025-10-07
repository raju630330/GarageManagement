namespace GarageManagement.Server.Model;
    public class ResetPasswordRequest
{
    public string EmailOrUsername { get; set; } = default!;
    //public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    //public string NewPassword { get; set; } = default!;
}
