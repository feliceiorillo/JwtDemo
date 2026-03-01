using JwtDemo.Models;
using JwtDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        // Hard-coded demo credentials — replace with a real user store in production
        private const string DemoUsername = "admin";
        private const string DemoPassword = "password123";

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username != DemoUsername || request.Password != DemoPassword)
                return Unauthorized(new { message = "Invalid credentials." });

            var token = _jwtService.GenerateToken(request.Username);
            return Ok(new { token });
        }
    }
}
