using Application.Services.Abstract;
using Domain.Dtos;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;

namespace Application.Consumers;
public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
{
    private readonly ILogger<IOrderRequestCompletedEvent> _logger;
    private readonly IOrderService _orderService;
    public OrderRequestCompletedEventConsumer(ILogger<IOrderRequestCompletedEvent> logger, IOrderService orderService)
    {
        this._logger = logger;
        this._orderService = orderService;
    }

    public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
    {
        var @event = context.Message;

        var order = await _orderService.GetByOrderId(@event.OrderId);
        if (order.Data != null)
        {
            var updateRequest = new OrderUpdateRequestDto()
            {
                OrderId = @event.OrderId,
                OrderStatus = Domain.Enums.OrderStatus.Successful,
                Message = "Order has been successfully processed!"
            };

            await _orderService.Update(updateRequest);

            _logger.LogInformation($"{updateRequest.Message} Order Id : {@event.OrderId}");
        }
    }
}