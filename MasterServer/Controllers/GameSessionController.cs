using System.Security.Claims;
using MasterServer.Data;
using MasterServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterServer.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GameSessionController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly ITicketService _ticketService;

    public GameSessionController(AppDbContext context, ITicketService ticketService)
    {
        _context = context;
        _ticketService = ticketService;
    }

    [HttpGet("enter")]
    public async Task<IActionResult> Enter()
    {

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out long userId))
        {
            return Unauthorized("Something went wrong..");
        }

        var character = await _context.Characters.FirstOrDefaultAsync(c => c.UserId == userId);
        if (character == null)
        {
            return NotFound("Character not found.");
        }

        var ticket = _ticketService.IssueTicket(userId, character.Id);

        return Ok(new
        {
            Ticket = ticket,
            UdpIp = "127.0.0.1",
            Port = 29555
        });

    }

}