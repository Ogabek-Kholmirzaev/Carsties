using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer(ILogger<AuctionCreatedFaultConsumer> logger) : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        logger.LogInformation("--> Consuming faulty creation");

        var exception = context.Message.Exceptions.First();
        if (exception.ExceptionType == "System.ArgumentException")
        {
            context.Message.Message.Model = "FooBar";
            await context.Publish(context.Message.Message);
        }
        else
        {
            logger.LogInformation("Not an argument exception - update error dashboard somewhere");
        }
    }
}