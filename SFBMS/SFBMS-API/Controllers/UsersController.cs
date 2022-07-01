using BusinessObject;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult> Login()
        {
            string? uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (uid != null)
            {
                User? user = await userRepository.Get(uid);
                if (user != null)
                {
                    try
                    {
                        UserRecord? userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                        bool isUserRecordExisted = userRecord != null;
                        return Ok(user);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.StackTrace);
                    }
                }
                else // user is null
                {
                    try
                    {
                        UserRecord? userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                        string? email = userRecord?.Email;
                        string? name = String.IsNullOrWhiteSpace(userRecord?.DisplayName) ? email : userRecord?.DisplayName;
                        User newUser = new User
                        {
                            Id = uid,
                            Name = name,
                            IsAdmin = 0,
                            Email = email ?? uid,
                            Password = "",
                        };
                        await userRepository.Add(newUser);
                        return Ok(newUser);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.StackTrace);
                    }
                }
            }
            return Unauthorized();
        }
    }
}
