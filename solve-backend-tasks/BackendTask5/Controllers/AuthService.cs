using BackendTask5.Model;
using BackendTasks5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendTask5.Services
{
    public class AuthService : IAuthService
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
            var user = _dbContext.users.SingleOrDefault(u => u.Username == username);
            if (user == null) return false;

            return VerifyPassword(password, user.password);
        }
        public string GenerateToken(string username)
        {
            var tokenKey = _config["JwtSettings:Key"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];
            var user = _dbContext.users.Include(u => u.role).SingleOrDefault(u => u.Username == username);


            var key = Encoding.UTF8.GetBytes(tokenKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, user.role.roleTitle)
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
