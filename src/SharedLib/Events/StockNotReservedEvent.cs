using SharedLib.Contracts;

namespace SharedLib.Events;

public class StockNotReservedEvent : IStockNotReservedEvent
{
    public StockNotReservedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }
    public long OrderId { get; set; }
    public string Reason { get; set; }
    public Guid CorrelationId { get; }

}