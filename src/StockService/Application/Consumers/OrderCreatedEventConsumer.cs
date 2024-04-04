using Application.Services.Abstract;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;
using SharedLib.Events;

namespace Application.Consumers;
public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
{
    private readonly IStockService _stockService;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public OrderCreatedEventConsumer(IStockService stockService,
        ILogger<OrderCreatedEventConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        this._stockService = stockService;
        this._logger = logger;
        this._publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
    {
        var @event = context.Message;
        var result = await _stockService.CheckStock(@event.OrderItems);
        if (result.IsSuccess)
        {
            for (int index = 0; index < @event.OrderItems.Count; index++)
            {
                var stock = (await _stockService.GetByProductId(@event.OrderItems[index].ProductId)).Data;
                if (stock != null)
                {
                    stock.Count -= @event.OrderItems[index].Count;
                    await _stockService.Update(new() { Count = stock.Count, ProductId = @event.OrderItems[index].ProductId });
                }
            }

            var stockReservedEvent = new StockReservedEvent(@event.CorrelationId)
            {
                OrderItems = @event.OrderItems,
            };

            await _publishEndpoint.Publish(stockReservedEvent);

            _logger.LogInformation($"Stock was reserved for Correlation Id: {@event.CorrelationId}");
        }
        else
        {
            var stockNotReserved = new StockNotReservedEvent(@event.CorrelationId) { Reason = "Not enough stock!"};
            
            await _publishEndpoint.Publish(stockNotReserved);

            _logger.LogError($"Stock was not reserved for Correlation Id: {@event.CorrelationId}");

        }
    }
}