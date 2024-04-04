using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;
public class StockReservedEvent : IStockReservedEvent
{
    public StockReservedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    public Guid CorrelationId { get; }

}