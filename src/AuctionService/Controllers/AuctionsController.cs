using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController(
    AuctionDbContext context,
    IMapper mapper,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAllAuctions(string? date)
    {
        var query = context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrWhiteSpace(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }
        
        return await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        return mapper.Map<AuctionDto>(auction);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction([FromBody] CreateAuctionDto auctionDto)
    {
        var auction = mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        await context.Auctions.AddAsync(auction);
        
        var dto = mapper.Map<AuctionDto>(auction);
        await publishEndpoint.Publish(mapper.Map<AuctionCreated>(dto));
        
        await context.SaveChangesAsync();
        
        return CreatedAtAction(
            nameof(GetAuctionById),
            new { id = dto.Id },
            dto);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAuction(Guid id, [FromBody] UpdateAuctionDto auctionDto)
    {
        var auction = await context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        if (auction.Seller != User.Identity.Name)
        {
            return Forbid();
        }

        auction.Item.Make = auctionDto.Make;
        auction.Item.Model = auctionDto.Model;
        auction.Item.Year = auctionDto.Year;
        auction.Item.Color = auctionDto.Color;
        auction.Item.Mileage = auctionDto.Mileage;

        await publishEndpoint.Publish(mapper.Map<AuctionUpdated>(auction));
        await context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await context.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        if (auction.Seller != User.Identity.Name)
        {
            return Forbid();
        }

        context.Auctions.Remove(auction);
        await publishEndpoint.Publish(new AuctionDeleted { Id = auction.Id.ToString() });
        await context.SaveChangesAsync();

        return Ok();
    }
}