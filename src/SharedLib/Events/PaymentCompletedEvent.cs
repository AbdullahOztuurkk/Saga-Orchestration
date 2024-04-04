using SharedLib.Contracts;

namespace SharedLib.Events;
public class PaymentCompletedEvent : IPaymentCompletedEvent
{
    public PaymentCompletedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}