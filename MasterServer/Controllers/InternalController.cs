using System;
using MasterServer.Data;
using MasterServer.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ValidateHandshake(TicketDto ticketDto)
    {
        
        var data = _ticketService.ConsumeTicket(ticketDto.Ticket);

        if (data == null) return Unauthorized("Invalid ticket.");



    }
    
    private record TicketDto(string Ticket);

}