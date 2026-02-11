
using System.Security.Claims;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;

namespace GarageManagement.Server.Repositories
{  
    public class HelperRepository : IHelperRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelperRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public long GetUserId()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return string.IsNullOrEmpty(userId) ? 0 : long.Parse(userId);
        }

        public string GetUsername()
        {
            return User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }

        public string GetEmail()
        {
            return User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }

        public string GetRole()
        {
            return User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }
    }

}
