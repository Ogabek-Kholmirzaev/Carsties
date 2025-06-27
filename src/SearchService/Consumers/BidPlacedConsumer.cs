using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class BidPlacedConsumer(ILogger<BidPlacedConsumer> logger) : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        logger.LogInformation("--> Consuming bid placed: " + context.Message.Id);
        
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (auction == null)
        {
            logger.LogError($"--> Auction {context.Message.AuctionId} not found");
            return;
        }

        if (context.Message.BidStatus.Contains("Accepted") && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }
    }
}