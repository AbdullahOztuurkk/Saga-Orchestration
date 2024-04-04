using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SagaStateMachineWorkerService.Data;

/// <summary>
/// OrderStateInstance is used for store state of events.
/// </summary>
public class OrderStateInstance : SagaStateMachineInstance
{
    /// <summary>
    /// Event Unique Id
    /// </summary>
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    #region Order
    public string BuyerId { get; set; }
    public long OrderId { get; set; }
    public DateTime CreatedDate { get; set; }
    #endregion

    #region PaymentMessage
    public string CardOwnerName { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string CardExpireMonth { get; set; }
    public string CardExpireYear { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    #endregion

    public override string ToString()
    {
        var sb = new StringBuilder();
        var properties = GetType().GetProperties();
        properties.ToList().ForEach(prop =>
        {
            var value = prop.GetValue(this, null);
            sb.AppendLine($"{prop.Name}:{value}");
        });
        sb.AppendLine("---------------");
        return sb.ToString();
    }
}
