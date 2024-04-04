using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;
using SharedLib.Events;

namespace Application.Consumers;
public class StockReservedRequestPaymentEventConsumer : IConsumer<IStockReservedRequestPaymentEvent>
{
    private readonly ILogger<StockReservedRequestPaymentEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public StockReservedRequestPaymentEventConsumer(ILogger<StockReservedRequestPaymentEventConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        this._logger = logger;
        this._publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
    {
        var balance = 2000m;
        var @event = context.Message;

        if (balance > @event.Payment.TotalPrice)
        {
            _logger.LogInformation($"{@event.Payment.TotalPrice} TL was withdrawn from credit card for correlation id = {@event.CorrelationId}");

            var paymentSuccessEvent = new PaymentCompletedEvent(@event.CorrelationId);

            await _publishEndpoint.Publish(paymentSuccessEvent);
        }
        else
        {
            _logger.LogError($"{@event.Payment.TotalPrice} TL was not withdrawn from credit card for correlation id = {@event.CorrelationId}");

            var paymentFailedEvent = new PaymentFailedEvent(@event.CorrelationId)
            {
                OrderItems = @event.OrderItems,
                Reason = "Not Enough Balance!"
            };

            await _publishEndpoint.Publish(paymentFailedEvent);
        }
    }
}