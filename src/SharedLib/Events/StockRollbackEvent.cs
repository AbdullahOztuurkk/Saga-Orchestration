using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;

public class StockRollbackEvent : IStockRollbackEvent
{
    public List<OrderItemMessage> OrderItems { get; set; }
}
