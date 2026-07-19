using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MasterServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        
        _configuration = configuration;

    }

    private LoginDTO _testDTO = new LoginDTO("Admin", "123123");
    
    [HttpPost("login")]
    
    public IActionResult Login( [FromBody]LoginDTO data)
    {
        
        if (data.Username == _testDTO.Username && data.Password == _testDTO.Password)
        {
            
            // TOKEN SETTINGS =======================================================
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = System.Text.Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            var key = new SymmetricSecurityKey(secretKey);

            // TOKEN DESCrIPTION =======================================================

            var tokenDescription = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new[]
                {
                    
                    new Claim(ClaimTypes.Name, data.Username)

                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

            };

            // TOKEN CREATE=================================================================
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString});

        }
        else return Unauthorized("Invalid data.");

    }

}

public record LoginDTO(string Username, string Password);