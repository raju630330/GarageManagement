using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseAuthorizationController : ControllerBase
    {
    }
}
