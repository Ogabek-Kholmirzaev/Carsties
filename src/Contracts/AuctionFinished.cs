namespace Contracts;

public class AuctionFinished
{
    public bool ItemSold { get; set; }
    public string AuctionId { get; set; } = null!;
    public string Winner { get; set; } = null!;
    public string Seller { get; set; } = null!;
    public int? Amount { get; set; }
}