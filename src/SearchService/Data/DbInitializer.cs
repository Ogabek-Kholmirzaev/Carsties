using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public static class DbInitializer
{
    public static async Task InitDbAsync(WebApplication app)
    {
        await DB.InitAsync(
            "SearchDb",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();
        
        using var scope = app.Services.CreateScope();
        var auctionSvcHttpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        var items = await auctionSvcHttpClient.GetItemsForSearchDbAsync();

        Console.WriteLine(items.Count + " returned from the auction service.");

        if (items.Count > 0)
        {
            await DB.SaveAsync(items);
        }
    }
}