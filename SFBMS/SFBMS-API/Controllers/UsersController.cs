using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Security.Claims;

namespace SFBMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        [HttpPost("login")]
        //[Authorize]
        public async Task<ActionResult> Login([FromHeader] string? fcmToken)
        {
            string? uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (uid != null)
            {
            }
            return Ok();
        }
    }
}
