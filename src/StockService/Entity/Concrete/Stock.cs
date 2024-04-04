using CoreLib.Entity.Concrete;

namespace Domain.Concrete;
public class Stock : AuditEntity
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}
