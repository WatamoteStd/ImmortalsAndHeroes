using System;
using System.Security.Claims;
using MasterServer.Data;
using MasterServer.DTO;
using MasterServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Characters;

namespace MasterServer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CharacterController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public CharacterController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")] 
    public async Task<IActionResult> CreateCharacter([FromBody]CharacterCreateRequestDto cDto)
    {
        
        // CHECK THE TOKEN
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out long userId))
        {
            return Unauthorized("Invalid token state");
        }
        
        bool isCharacterExists = await _context.Characters.AnyAsync(u => u.Name == cDto.Nickname);
        if (isCharacterExists) return BadRequest("Character with this nickname already exists.");

        bool isUserExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!isUserExists) return BadRequest("Something went wrong with your account. Restart the game please and try to login again");

        var newCharacter = new Character
        { 
            RegionId = 0,
            Name = cDto.Nickname,

            PosX = 0f,
            PosY = 1f,
            PosZ = 0f,

            UserId = userId,
            Type = cDto.Type,
        };

        _context.Characters.Add(newCharacter);
        await _context.SaveChangesAsync();

        var responseDto = new CharacterCreatedDto(
            newCharacter.Name,
            newCharacter.Silver,
            newCharacter.Type,
            newCharacter.Id
        );

        return Ok(responseDto);

    }

    [HttpGet("get")]
    public async Task<IActionResult> GetCharacter()
    {

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out long userId))
        {
            return Unauthorized("Invalid token state");
        }

        var character = await _context.Characters
            .Where(c => c.UserId == userId)
            .Select(c => new CharacterCreatedDto(
                c.Name,
                c.Silver,
                c.Type,
                c.Id
            ))
            .FirstOrDefaultAsync();

        if (character == null) return NotFound("You have no character. Create new one");

        return Ok(character);

    }

   
    
    private record CharacterCreatedDto(string Nickname, long Silver, EntityType Type, long Id);

}