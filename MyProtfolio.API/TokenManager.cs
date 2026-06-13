using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Model.Admin;
using Microsoft.IdentityModel.Tokens;

namespace  MyProtfolio.API.TokenManager{
    public class TokenManager
    {
        public string GenerateToken(Admin user)
        {
            var secret = Environment.GetEnvironmentVariable("JWT__Key")
                ?? throw new Exception("JWT_KEY is missing");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}