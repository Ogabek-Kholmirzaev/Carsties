using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionFinishedConsumer(
    IHubContext<NotificationHub> hubContext,
    ILogger<AuctionFinishedConsumer> logger) : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        logger.LogInformation("--> Consuming auction finished message");
        await hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}
