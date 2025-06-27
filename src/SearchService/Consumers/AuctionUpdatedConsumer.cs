using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer(IMapper mapper, ILogger<AuctionUpdatedConsumer> logger) : IConsumer<AuctionUpdated>
{
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        logger.LogInformation($"--> Consuming auction updated: {context.Message.Id}");
        
        var item = mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
            .Match(i => i.ID == context.Message.Id)
            .ModifyOnly(x => new
            {
                x.Color,
                x.Make,
                x.Model,
                x.Year,
                x.Mileage
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb.");
        }
    }
}