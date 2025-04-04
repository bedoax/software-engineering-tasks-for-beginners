using BackendTask3.Model;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }
        [Route("")]
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_authService.validateUser(login.Username, login.Password))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }
            var token = _authService.GenerateToken(login.Username);
            return Ok(token);
        }
    }
}
