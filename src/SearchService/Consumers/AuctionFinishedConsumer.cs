using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer(ILogger<AuctionFinishedConsumer> logger) : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        logger.LogInformation($"--> Consuming auction finished {context.Message.AuctionId}");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        if (auction == null)
        {
            logger.LogError($"--> Auction {context.Message.AuctionId} not found");
            return;
        }
        
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount ?? 0;
        }
        
        auction.Status = "Finished";
        await auction.SaveAsync();
    }
}