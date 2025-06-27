using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer(AuctionDbContext dbContext, ILogger<BidPlacedConsumer> logger) : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        logger.LogInformation("--> Consuming bid placed");

        var auction = await dbContext.Auctions.FindAsync(context.Message.AuctionId);
        if (auction == null)
        {
            logger.LogError($"--> Auction {context.Message.AuctionId} not found");
            return;
        }

        if (auction.CurrentHighBid != null ||
            context.Message.BidStatus.Contains("Accepted") &&
            context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await dbContext.SaveChangesAsync();
        }
}
}