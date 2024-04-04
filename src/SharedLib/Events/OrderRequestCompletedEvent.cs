using SharedLib.Contracts;

namespace SharedLib.Events;

public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
{
    public long OrderId { get; set; }
}

public class OrderRequestFailedEvent : IOrderRequestFailedEvent
{
    public long OrderId { get; set; }
    public string Reason { get; set; }
}
