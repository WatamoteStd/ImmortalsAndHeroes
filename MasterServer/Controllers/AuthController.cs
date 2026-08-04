using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MasterServer.Data;
using MasterServer.DTO;
using MasterServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace MasterServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        
        _context = context;
        _configuration = configuration;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        
        bool userExists = await _context.Users.AnyAsync(u => u.Login == dto.Username || u.Email == dto.Email);

        if (userExists)
        {
            
            return BadRequest("Player with this username or email already exists. Try another one.");

        }
     
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User
        {
            Login = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

         _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Account created succesfully!");

    
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Username);

        if (user == null)
        {
            
            return BadRequest("Invalid user or password.");

        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid) return BadRequest("Invalid user or password.");

        string jwtToken = GenerateJwtToken(user);
        

        var response = new LoginResponseDto
        {
            Username = user.Login,
            UserId = user.Id,
            CreatedAt = user.CreatedAt,
            Token = jwtToken
        };

        return Ok(response);

    }

    private string GenerateJwtToken(User user)
    {

        var jwtSecret = _configuration["JwtSettings:Secret"] // GET THE CONFIGURATION
                        ?? throw new InvalidOperationException("JWT secret miss");
        var key = Encoding.UTF8.GetBytes(jwtSecret);
        
        // Injecting data in token ====================================================

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        // CREATE TOKEN SETTINGS ============================

        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        
        // GENERATE TOKEN STRING
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }

}

    

