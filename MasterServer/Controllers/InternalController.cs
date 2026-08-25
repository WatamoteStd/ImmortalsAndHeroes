using System;
using MasterServer.Data;
using MasterServer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Characters;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;

namespace MasterServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InternalController : ControllerBase
{
    
    private readonly AppDbContext _context;
    private readonly ITicketService _ticketService;

    public InternalController(AppDbContext context, ITicketService ticket)
    {
        _context = context;
        _ticketService = ticket;
    }

    [HttpPost("validate-handshake")]
    public async Task<IActionResult> ValidateHandshake([FromBody] TicketDto ticketDto)
    {
        
        var data = _ticketService.ConsumeTicket(ticketDto.Ticket);

        if (data == null) return Unauthorized("Invalid ticket.");

        var responseDto = await _context.Characters
        .Where(c => c.Id == data.CharacterId && c.UserId == data.UserId)
        .Select(c => new HandshakeResponseDto()
        {
            Id = c.Id,
            RegionId = c.RegionId,
            Name = c.Name,
            PosX = c.PosX,
            PosY = c.PosY,
            PosZ = c.PosZ,
            UserId = c.UserId,
            Type = c.Type,
            CurrentHp = c.CurrentHp,
            CurrentMp = c.CurrentMp,
            Exp = c.Exp,
            Silver = c.Silver
        })
        .FirstOrDefaultAsync();

        if (responseDto == null) return NotFound("Character not found");

        return Ok(responseDto);
        



    }
    
    public record TicketDto(string Ticket);

}