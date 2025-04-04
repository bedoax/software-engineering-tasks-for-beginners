using BackendTask3.Model;
using BackendTasks.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RegisterController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("")]
        [HttpPost]
        public IActionResult Register([FromBody] Register register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.users.SingleOrDefault(u => u.username == register.username);
            if (isExist != null)
            {
                return BadRequest(new { message = "User already exists" });
            }
            var user = new User
            {
                username = register.username,
                password = register.password
            };
            _dbContext.users.Add(user);
            _dbContext.SaveChanges();
            return Ok(new { register.username});
        }
    }
}
