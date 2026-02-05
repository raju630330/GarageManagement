using System.Security.Claims;
using Microsoft.AspNetCore.Http;



namespace GarageManagement.Server.Helpers
{
    
    public interface IJwtUserContext
    {
        string GetUserId();
        string GetUserName();
        string GetUserRole();
    }
    public class JwtUserContext : IJwtUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? "System";
        }

        public string GetUserName()
        {
            return User?.FindFirstValue(ClaimTypes.Name)
                   ?? "System";
        }

        public string GetUserRole()
        {
            return User?.FindFirstValue(ClaimTypes.Role)
                   ?? string.Empty;
        }
    }

}
