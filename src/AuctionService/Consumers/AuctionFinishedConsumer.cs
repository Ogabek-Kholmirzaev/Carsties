using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer(
    AuctionDbContext dbContext,
    ILogger<AuctionFinished> logger) 
    : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var auction = await dbContext.Auctions.FindAsync(context.Message.AuctionId);
        if (auction == null)
        {
            logger.LogError($"--> Auction {context.Message.AuctionId} not found");
            return;
        }
        
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        auction.Status = auction.SoldAmount > auction.ReservePrice
            ? EStatus.Finished
            : EStatus.ReserveNotMet;

        await dbContext.SaveChangesAsync();
    }
}