using AuctionService.Data;
using AuctionService.Protos;
using Grpc.Core;

namespace AuctionService.Services;

public class GrpcAuctionService(
    AuctionDbContext dbContext,
    ILogger<GrpcAuctionService> logger) : GrpcAuction.GrpcAuctionBase
{
    public override async Task<GrpcAuctionResponse> GetAuction(
        GetAuctionRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("==> Received Grpc request for auction");

        var auction = await dbContext.Auctions.FindAsync(Guid.Parse(request.Id))
            ?? throw new RpcException(new Status(StatusCode.NotFound, $"Auction not found: {request.Id}"));

        var response = new GrpcAuctionResponse
        {
            Auction = new GrpcAuctionModel
            {
                Id = auction.Id.ToString(),
                Seller = auction.Seller,
                AuctionEnd = auction.AuctionEnd.ToString(),
                ReservePrice = auction.ReservePrice
            }
        };

        return response;
    }
}
