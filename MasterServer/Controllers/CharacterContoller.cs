using System;
using MasterServer.Data;
using MasterServer.DTO;
using MasterServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Characters;

namespace MasterServer.Controllers;


[ApiController]
[Route("api/[controller]")]
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
        
        bool isCharacterExists = await _context.Characters.AnyAsync(u => u.Name == cDto.Nickname);
        if (isCharacterExists) return BadRequest("Character with this nickname already exists.");

        bool isUserExists = await _context.Users.AnyAsync(u => u.Id == cDto.UserId);
        if (!isUserExists) return BadRequest("Something went wrong with your account. Restart the game please and enter again to your account");

        var newCharacter = new Character
        { 
            RegionId = 0,
            Name = cDto.Nickname,

            PosX = 0f,
            PosY = 1f,
            PosZ = 0f,

            UserId = cDto.UserId,
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

    private record CharacterCreatedDto(string Nickname, long Silver, CharacterType Type, long Id);

}