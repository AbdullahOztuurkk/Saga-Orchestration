using MassTransit;

namespace SharedLib.Contracts;

//CorrelationId has been added with CorrelatedBy interface.
//It used for update states in state machine.
public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
{

}
