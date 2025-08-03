namespace BiddingService.DTOs;

public class BidDto
{
    public string Id { get; set; } = null!;
    public string AuctionId { get; set; } = null!;
    public string Bidder { get; set; } = null!;
    public DateTime BidTime { get; set; }
    public int Amount { get; set; }
    public string BidStatus { get; set; } = null!;
}
