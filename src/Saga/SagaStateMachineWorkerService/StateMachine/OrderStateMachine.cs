using MassTransit;
using SagaStateMachineWorkerService.Data;
using SharedLib;
using SharedLib.Contracts;
using SharedLib.Events;
using SharedLib.ValueObject;

namespace SagaStateMachineWorkerService.StateMachine;
/// <summary>
/// Base class provides to update event status and manage entire distributed transactions.
/// </summary>
public partial class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    #region States
    //Second state after initial state
    public State OrderCreated { get; private set; }
    public State StockReserved { get; private set; }
    public State StockNotReserved { get; private set; }
    public State PaymentSucceeded { get; private set; }
    public State PaymentFailed { get; private set; }
    #endregion

    #region Events
    public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
    public Event<IStockReservedEvent> StockReservedEvent { get; set; }
    public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
    public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }
    #endregion

    public OrderStateMachine()
    {
        //Set initial state
        InstanceState(x => x.CurrentState);

        //Compare OrderIds between dto and db record. If doesnt exist, assign new guid. It happens only OrderCreatedRequestEvent published.
        Event(() => OrderCreatedRequestEvent, x => x.CorrelateBy<long>(y => y.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Event(() => PaymentFailedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        //Mapping dto to db record when OrderCreatedRequestEvent has been published
        Initially(When(OrderCreatedRequestEvent)
            .Then(context =>
            {
                context.Saga.BuyerId = context.Message.BuyerId;
                context.Saga.CardNumber = context.Message.Payment.CardNumber;
                context.Saga.CVV = context.Message.Payment.CVV;
                context.Saga.CardOwnerName = context.Message.Payment.CardOwnerName;
                context.Saga.CardExpireMonth = context.Message.Payment.CardExpireMonth;
                context.Saga.CardExpireYear = context.Message.Payment.CardExpireYear;
                context.Saga.CreatedDate = DateTime.UtcNow.AddHours(3);
                context.Saga.TotalPrice = context.Message.Payment.TotalPrice;
            })
            .TransitionTo(OrderCreated)//Change status after mapping
            .Publish(context => new OrderCreatedEvent(context.Saga.CorrelationId) { OrderItems = context.Message.OrderItems })
            .Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent After:\n{context.Saga}");
            }));

        During(OrderCreated,
            //State set as StockReserved while StockReservedEvent published and current state is OrderCreated
            When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{QueueNames.PaymentStockReservedRequest}"), context => new StockReservedRequestPaymentEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                    Payment = new PaymentMessage
                    {
                        CardExpireMonth = context.Saga.CardExpireMonth,
                        CardExpireYear = context.Saga.CardExpireYear,
                        CVV = context.Saga.CVV,
                        CardNumber = context.Saga.CardNumber,
                        CardOwnerName = context.Saga.CardOwnerName,
                        TotalPrice = context.Saga.TotalPrice
                    },
                    BuyerId = context.Saga.BuyerId,
                })
                .Then(context => { Console.WriteLine($"StockReservedEvent After:\n{context.Saga}"); }),
            //State set as PaymentFailed while StockNotReservedEvent published and current state is OrderCreated
            When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(context => new OrderRequestFailedEvent()
                { 
                    OrderId = context.Saga.OrderId,
                    Reason = context.Message.Reason
                })
                .Then(context => { Console.WriteLine($"StockNotReservedEvent After:\n{context.Saga}"); }));

        During(StockReserved,
            //State set as PaymentSucceeded while PaymentCompletedEvent published and current state is StockReserved
            When(PaymentCompletedEvent)
                .TransitionTo(PaymentSucceeded)
                .Publish(context => new OrderRequestCompletedEvent { OrderId = context.Saga.OrderId })
                .Then(context => { Console.WriteLine($"PaymentCompletedEvent After:\n{context.Saga}"); })
                .Finalize(),
            //State set as PaymentFailed while PaymentFailedEvent published and current state is StockReserved
            When(PaymentFailedEvent)
                .TransitionTo(PaymentFailed)
                .Publish(context => new OrderRequestFailedEvent()
                {
                    OrderId = context.Saga.OrderId,
                    Reason = context.Message.Reason
                })
                .Send(new Uri($"queue:{QueueNames.StockRollback}"), context => new StockRollbackEvent() { OrderItems = context.Message.OrderItems })
                .Then(context => { Console.WriteLine($"PaymentFailedEvent After:\n{context.Saga}"); }));

        //Delete all db record that state is Final if event series are completed successfully
        SetCompletedWhenFinalized();
    }
}