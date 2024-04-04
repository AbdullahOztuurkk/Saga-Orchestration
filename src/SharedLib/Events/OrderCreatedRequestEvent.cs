using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;
public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
{
    public long OrderId { get; set; }
    public string BuyerId { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    public PaymentMessage Payment { get; set; }
}
