using SharedLib.ValueObject;

namespace SharedLib.Contracts;
public interface IOrderCreatedRequestEvent
{
    public long OrderId { get; set; }
    public string BuyerId { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } 
    public PaymentMessage Payment { get; set; }
}
