using MassTransit;
using SharedLib.ValueObject;

namespace SharedLib.Contracts;
public interface IStockReservedRequestPaymentEvent : CorrelatedBy<Guid>
{
    public PaymentMessage Payment { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; }
}
