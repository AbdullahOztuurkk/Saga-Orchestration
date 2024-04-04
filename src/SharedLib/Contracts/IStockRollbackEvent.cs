using SharedLib.ValueObject;

namespace SharedLib.Contracts;

public interface IStockRollbackEvent
{
    public List<OrderItemMessage> OrderItems { get; set; }
}