

using BiddingService.Models;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Services;

public class CheckAuctionFinishedService(
    ILogger<CheckAuctionFinishedService> logger,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting check for finished auctions");
        stoppingToken.Register(() => logger.LogInformation("==> Auction check is stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAuctionsAync(stoppingToken);
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task CheckAuctionsAync(CancellationToken stoppingToken)
    {
        var finishedAuctions = await DB.Find<Auction>()
            .Match(a => a.AuctionEnd <= DateTime.UtcNow)
            .Match(a => a.Finished == false)
            .ExecuteAsync(stoppingToken);

        logger.LogInformation($"==> Found {finishedAuctions.Count} auctions that have completed");

        if (finishedAuctions.Count == 0)
        {
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        foreach (var auction in finishedAuctions)
        {
            auction.Finished = true;
            await auction.SaveAsync(null, stoppingToken);

            var winningBid = await DB.Find<Bid>()
                .Match(bid => bid.AuctionId == auction.ID)
                .Match(b => b.BidStatus == EBidStatus.Accepted)
                .Sort(x => x.Descending(b => b.Amount))
                .ExecuteFirstAsync(stoppingToken);

            await publishEndpoint.Publish(new AuctionFinished
            {
                ItemSold = winningBid != null,
                AuctionId = auction.ID,
                Winner = winningBid?.Bidder ?? string.Empty,
                Seller = auction.Seller,
                Amount = winningBid?.Amount
            }, stoppingToken);
        }
    }
}
