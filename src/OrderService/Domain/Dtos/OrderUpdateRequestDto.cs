using Domain.Enums;

namespace Domain.Dtos;

public class OrderUpdateRequestDto
{
    public long OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string Message { get; set; }
}