using BackendTask5.Model;
using BackendTasks5.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackendTask5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("")]
        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExist = _dbContext.Roles.SingleOrDefault(r => r.roleTitle == role.roleTitle);
            if (isExist != null)
            {
                return BadRequest(new { message = "Role already exists" });
            }
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();
            return Ok($"Role {role.roleTitle} added successfully");
        }
        [Route("")]
        [HttpGet]
        public IActionResult GetRoles()
        {
            try
            {
                return Ok(_dbContext.Roles.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving roles.", error = ex.Message });
            }
        }
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetRole(int id)
        {
            try
            {
                var role = _dbContext.Roles.FirstOrDefault(r => r.Id == id);
                if (role == null)
                {
                    return NotFound(new { message = "The role was not found." });
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the role.", error = ex.Message });
            }
        }
/*        [Route("{id}")]
        [HttpPut]

        public IActionResult EditRole(int id , Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var roleInDb = _dbContext.Roles.FirstOrDefault(r => r.Id == id);
            if (roleInDb == null)
            {
                return NotFound(new { message = "The role was not found." });
            }
            roleInDb.roleTitle = role.roleTitle;
            _dbContext.Roles.Update(roleInDb);
            _dbContext.SaveChanges();
            return Ok($"Role {role.roleTitle} updated successfully");
        }*/
/*        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteRole(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _dbContext.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return NotFound(new { message = "The role was not found." });
            }
            _dbContext.Roles.Remove(role);
            _dbContext.SaveChanges();
            return Ok($"Role {role.roleTitle} deleted successfully");

        }*/
    }
}
