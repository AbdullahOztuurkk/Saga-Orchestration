using Application.Services.Abstract;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;
using SharedLib.Events;

namespace Application.Consumers;

public class StockRollbackConsumer : IConsumer<IStockRollbackEvent>
{
    private readonly ILogger<StockRollbackConsumer> _logger;
    private readonly IStockService _stockService;
    public StockRollbackConsumer(ILogger<StockRollbackConsumer> logger, IStockService stockService)
    {
        this._logger = logger;
        this._stockService = stockService;
    }

    public async Task Consume(ConsumeContext<IStockRollbackEvent> context)
    {
        var @event = context.Message;
        for(int index = 0; index < @event.OrderItems.Count; index++)
        {
            var stock = (await _stockService.GetByProductId(@event.OrderItems[index].ProductId)).Data;
            if(stock != null) 
            {
                stock.Count += @event.OrderItems[index].Count;
                await _stockService.Update(new() { Count = stock.Count, ProductId = @event.OrderItems[index].ProductId });
            }
        }

        _logger.LogInformation($"Stock was released");
    }
}
