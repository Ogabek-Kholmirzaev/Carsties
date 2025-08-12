using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionCreatedConsumer(
    IHubContext<NotificationHub> hubContext,
    ILogger<AuctionCreatedConsumer> logger) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        logger.LogInformation("--> Consuming auction created message");
        await hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
    }
}
