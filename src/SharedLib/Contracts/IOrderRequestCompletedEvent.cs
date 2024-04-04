namespace SharedLib.Contracts;

public interface IOrderRequestCompletedEvent 
{
    public long OrderId { get; set; }
}
