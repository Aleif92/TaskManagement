using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using TaskManagement.Repositories;
using TaskManagement.Services;
using TaskManagement.Models;



namespace TaskManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly string _jwtSecret = "your_secret_key_here"; // 🔑 Replace with a strong secret key

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ User Registration Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Check if the username already exists
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Username already exists!" });

            // Hash the password before saving it
            user.PasswordHash = AuthServices.HashPassword(user.PasswordHash);

            // Save the new user
            await _userRepository.RegisterUserAsync(user);
            return Ok(new { message = "User registered successfully!" });
        }

        // ✅ User Login Endpoint (Fixed)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
            if (existingUser == null || !AuthServices.VerifyPassword(loginRequest.PasswordHash, existingUser.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password!" });

            // 🔑 Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,
                              existingUser.Username),
                    new Claim(ClaimTypes.Role, existingUser.Role) // ✅ Keep Role inside the token (optional)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}
