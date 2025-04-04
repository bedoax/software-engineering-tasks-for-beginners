using BackendTask4.Model;
using BackendTasks.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace BackendTask4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.Users.SingleOrDefault(u => u.Username == user.Username);
            if (isExist != null)
            {
                return BadRequest(new { message = "User already exists" });
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok(new { user });
        }
    }



}


