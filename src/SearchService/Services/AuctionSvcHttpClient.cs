using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
{
    public async Task<List<Item>> GetItemsForSearchDbAsync()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        var items = await httpClient.GetFromJsonAsync<List<Item>>(
            $"{configuration["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}");

        return items ?? new List<Item>();
    }
}