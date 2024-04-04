namespace SharedLib.Contracts;

public interface IOrderRequestFailedEvent
{
    public long OrderId { get; set; }
    public string Reason { get; set; }
}