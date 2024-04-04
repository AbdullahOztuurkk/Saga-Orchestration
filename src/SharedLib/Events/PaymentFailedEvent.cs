using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;

public class PaymentFailedEvent : IPaymentFailedEvent
{
    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    public string Reason { get; set; }
    public long OrderId { get; set; }
    public Guid CorrelationId { get; }

    public PaymentFailedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }
}
