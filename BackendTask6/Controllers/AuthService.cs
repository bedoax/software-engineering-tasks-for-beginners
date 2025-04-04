using BackendTask6.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendTask6.Controllers
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
        public AuthService(ApplicationDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }
        public bool validateUser(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.username == username);
            if (user == null) return false;
            return VerifyPassword(password, user.Password);
        }
        public bool validateRole(string username, string role)
        {
            var user = _dbContext.Users.Include(u => u.role).SingleOrDefault(u => u.username == username);
            if (user == null) return false;
            return user.role.name == role;
        }
        public string GenerateToken(string username)
        {
            var tokenKey = _config["JwtSettings:Key"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];
           var user = _dbContext.Users.Include(u => u.role).SingleOrDefault(u => u.username == username);
            var key = Encoding.UTF8.GetBytes(tokenKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, user.role.name)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private static bool VerifyPassword(string enteredPassword, string storedpassword)
        {
            return (enteredPassword) == storedpassword;
        }
    }
}
