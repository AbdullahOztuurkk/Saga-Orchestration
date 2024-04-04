using MassTransit;

namespace SharedLib.Contracts;

//CorrelationId has been added with CorrelatedBy interface.
//It used for update states in state machine.
public interface IStockNotReservedEvent : CorrelatedBy<Guid>
{
    public string Reason { get; set; }
}
