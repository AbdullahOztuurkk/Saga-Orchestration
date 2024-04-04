using MassTransit;
using SharedLib.ValueObject;

namespace SharedLib.Contracts;

//CorrelationId has been added with CorrelatedBy interface.
//It used for update states in state machine.
public interface IPaymentFailedEvent : CorrelatedBy<Guid>
{
    public string Reason { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; }
}
