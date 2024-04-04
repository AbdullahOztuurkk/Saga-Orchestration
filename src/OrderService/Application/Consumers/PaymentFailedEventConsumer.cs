using Application.Services.Abstract;
using Domain.Dtos;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedLib.Contracts;
using SharedLib.Events;

namespace Application.Consumers;

public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
{
    private readonly ILogger<OrderRequestFailedEventConsumer> _logger;
    private readonly IOrderService _orderService;

    public OrderRequestFailedEventConsumer(ILogger<OrderRequestFailedEventConsumer> logger, IOrderService orderService)
    {
        this._logger = logger;
        this._orderService = orderService;
    }

    public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
    {
        var @event = context.Message;

        var order = await _orderService.GetByOrderId(@event.OrderId);
        if (order.Data != null)
        {
            var updateRequest = new OrderUpdateRequestDto()
            {
                OrderId = @event.OrderId,
                OrderStatus = Domain.Enums.OrderStatus.Failed,
                Message = "Order has been failed!"
            };
            await _orderService.Update(updateRequest);

            _logger.LogError($"{updateRequest.Message} - Order Id : {@event.OrderId}");
        }
    }
}
