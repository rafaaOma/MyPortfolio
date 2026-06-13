using Microsoft.AspNetCore.Mvc;
using MyProtfolio.API.Data;
using Model.Admin;
using MyProtfolio.API.TokenManager;

namespace loginController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class loginVerifierController : ControllerBase
    {
        private readonly PortfolioDbContext _context;
        public loginVerifierController(PortfolioDbContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var user = _context.Admins.FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null)
                return Unauthorized("Invalid username");

            var isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!isValid)
                return Unauthorized("Invalid password");

            var tokenManager = new TokenManager();
            var token = tokenManager.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
