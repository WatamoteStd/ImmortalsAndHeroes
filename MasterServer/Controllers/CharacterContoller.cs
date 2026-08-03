using System;
using MasterServer.Data;
using MasterServer.DTO;
using MasterServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    /*[HttpPost("create")] 
    public async Task<IActionResult> CreateCharacter([FromBody]CharacterCreateRequestDto cDto)
    {
        
        bool isCharacterExists = await _context.Characters.AnyAsync(u => u.Name == cDto.Nickname);

        if (isCharacterExists) return BadRequest("Character with this nickname already exists.");

        var character = new Character
        {
            
        };

    }*/

}