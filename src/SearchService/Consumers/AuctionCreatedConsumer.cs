﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        logger.LogInformation("--> Consuming auction created: " + context.Message.Id);

        var item = mapper.Map<Item>(context.Message);
        if (item.Model == "Foo")
        {
            throw new ArgumentException("Cannot sell cars with name of Foo");
        }
        
        await item.SaveAsync();
    }
}