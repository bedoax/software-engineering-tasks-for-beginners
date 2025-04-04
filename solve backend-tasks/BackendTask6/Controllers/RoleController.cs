using BackendTask6.Data;
using BackendTask6.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendTask6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.id == id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole( string roleName)
        {
            if (roleName == null) return BadRequest("Role cannot be null");
            if (string.IsNullOrEmpty(roleName)) return BadRequest("Role name cannot be empty");
           var role = new Role
            {
                name = roleName
           };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoleById), new { id = role.id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, string newRoleName)
        {
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null) return NotFound();

            existingRole.name = newRoleName;

            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();
            return Ok(existingRole);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return Ok(role);
        }
    }
}
