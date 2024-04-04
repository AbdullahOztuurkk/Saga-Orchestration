using SharedLib.Contracts;
using SharedLib.ValueObject;

namespace SharedLib.Events;

public class StockReservedRequestPaymentEvent : IStockReservedRequestPaymentEvent
{
    public StockReservedRequestPaymentEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public string BuyerId { get; set; }
    public PaymentMessage Payment { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    public Guid CorrelationId { get; }

}