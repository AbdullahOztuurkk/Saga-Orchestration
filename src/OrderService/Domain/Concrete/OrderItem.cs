using CoreLib.Entity.Concrete;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Concrete;

public class OrderItem : AuditEntity
{
    public int ProductId { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public Order Order { get; set; }
    public long OrderId { get; set; }
    public int Count { get; set; }
}
