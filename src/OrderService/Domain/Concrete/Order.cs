using CoreLib.Entity.Concrete;
using Domain.Enums;

namespace Domain.Concrete;

public class Order : AuditEntity
{
    public string BuyerId { get; set; }
    public Address Address { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderStatus Status { get; set; } = OrderStatus.Suspended;
    public string? ResponseMessage { get; set; }
}