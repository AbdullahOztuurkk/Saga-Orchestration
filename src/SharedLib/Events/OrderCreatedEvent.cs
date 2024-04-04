using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;
public class OrderCreatedEvent : IOrderCreatedEvent
{
    public OrderCreatedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public List<OrderItemMessage> OrderItems { get; set; }
    public Guid CorrelationId { get; }
}
