using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDto
{
    [Required]
    public int ReservePrice { get; set; }

    [Required]
    public DateTime AuctionEnd { get; set; }


    [Required]
    public string Make { get; set; } = null!;

    [Required]
    public string Model { get; set; } = null!;

    [Required]
    public int Year { get; set; }

    [Required]
    public string Color { get; set; } = null!;

    [Required]
    public int Mileage { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;
}
