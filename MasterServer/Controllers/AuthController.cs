using System;
using MasterServer.Data;
using MasterServer.DTO;
using MasterServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MasterServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        
        _context = context;

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


}

    

