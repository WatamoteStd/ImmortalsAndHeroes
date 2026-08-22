

using MasterServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects.Market;

namespace MasterServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MarketController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public MarketController(AppDbContext appDb)
    {
        _context = appDb;
    }

    [HttpGet]
    public async Task<IActionResult> TakeAll([FromQuery] long regionId)
    {
        
        var items = await _context.GlobalMarket.AsNoTracking().Where(x => x.LocationId == regionId && x.IsSold == false)
            .Take(50)
            .Select(x => new MarketItemDto
            {
                OrderId = x.Id,
                ItemType = x.ItemType,
                Quality = x.Quality,
                SellerName = x.SellerName,
                PricePerUnit = x.PricePerUnit,
                Count = x.Count,
                IsCityOrder = x.IsCityOrder
            })
            .ToListAsync();

        return Ok(items);
    }


}