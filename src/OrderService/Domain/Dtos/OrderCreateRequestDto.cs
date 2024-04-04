namespace Domain.Dtos;
public class OrderCreateRequestDto
{
    public string BuyerId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public PaymentDto Payment { get; set; }
    public AddressDto Address { get; set; }
}

public class OrderItemDto 
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
}

public class PaymentDto
{
    public string CardOwnerName { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string CardExpireMonth { get; set; }
    public string CardExpireYear { get; set; }
}

public class AddressDto
{
    public string Line { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
}
