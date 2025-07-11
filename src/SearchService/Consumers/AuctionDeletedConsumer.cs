﻿using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer(ILogger<AuctionDeletedConsumer> logger) : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        logger.LogInformation($"--> Consuming AuctionDeleted: {context.Message.Id}");

        var result = await DB.DeleteAsync<Item>(context.Message.Id);
        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction from mongodb.");
        }
    }
}