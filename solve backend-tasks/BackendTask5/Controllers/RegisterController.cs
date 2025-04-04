using BackendTask5.Model;
using BackendTasks5.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask5.Controllers
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
            var isExist = _dbContext.users.SingleOrDefault(u => u.Username == register.username);
            if (isExist != null)
            {
                return BadRequest(new { message = "User already exists" });
            }
            var role = _dbContext.Roles.FirstOrDefault(x => x.roleTitle == register.role);
            var user = new User
            {
                Username = register.username,
                password = register.password,
                role = role,
                RoleId = role.Id
            };
            _dbContext.users.Add(user);
            _dbContext.SaveChanges();
            return Ok(new { register.username});
        }
    }
}
