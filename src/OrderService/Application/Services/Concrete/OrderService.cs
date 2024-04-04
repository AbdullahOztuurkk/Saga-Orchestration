using Application.Services.Abstract;
using CoreLib.ResponseModel;
using Domain.Concrete;
using Domain.Dtos;
using MassTransit;
using SharedLib;
using SharedLib.Contracts;
using SharedLib.Events;
using SharedLib.ValueObject;
using Order = Domain.Concrete.Order;

namespace Application.Services.Concrete;
public class OrderService : BaseService, IOrderService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    public OrderService(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }
    public async Task<BaseResponse> CreateOrder(OrderCreateRequestDto request)
    {
        var newOrder = new Order
        {
            BuyerId = request.BuyerId,
            Address = new Address
            {
                Line = request.Address.Line,
                Province = request.Address.Province,
                District = request.Address.District,
            },
            CreateDate = DateTime.UtcNow.AddHours(3),
        };

        request.OrderItems.ForEach(item =>
        {
            newOrder.OrderItems.Add(new()
            {
                Count = item.Count,
                ProductId = item.ProductId,
                Price = item.Price,
            });
        });

        await Db.GetDefaultRepo<Order>().InsertAsync(newOrder);
        await Db.GetDefaultRepo<OrderItem>().SaveChanges();
        await Db.GetDefaultRepo<Order>().SaveChanges();
        Db.Commit();

        var orderCreatedEvent = new OrderCreatedRequestEvent
        {
            BuyerId = newOrder.BuyerId,
            OrderId = newOrder.Id.ToInt(),
            Payment = new PaymentMessage
            {
                CardExpireMonth = request.Payment.CardExpireMonth,
                CardExpireYear = request.Payment.CardExpireYear,
                CardNumber = request.Payment.CardNumber,
                CardOwnerName = request.Payment.CardOwnerName,
                CVV = request.Payment.CVV,
                TotalPrice = request.OrderItems.Sum(item => item.Price * item.Count),
            }
        };

        request.OrderItems.ForEach(item =>
        {
            orderCreatedEvent.OrderItems.Add(new OrderItemMessage { Count = item.Count, ProductId = item.ProductId });
        });

        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueNames.OrderSaga}"));
        
        await sendEndpoint.Send<IOrderCreatedRequestEvent>(orderCreatedEvent);

        return new BaseResponse();
    }

    public async Task<BaseResponse> GetByOrderId(long OrderId)
    {
        var order = await Db.GetDefaultRepo<Order>().GetAsync(x => x.Id == OrderId);
        if(order == null)
            return new BaseResponse() { IsSuccess = false};

        return new BaseResponse { Data = order };
    }

    public async Task<BaseResponse> Update(OrderUpdateRequestDto request)
    {
        var order = await Db.GetDefaultRepo<Order>().GetAsync(x => x.Id == request.OrderId);
        if (order == null)
            return new BaseResponse() { IsSuccess = false };

        order.Status = request.OrderStatus;
        order.ResponseMessage = request.Message;

        await Db.GetDefaultRepo<Order>().SaveChanges();
        Db.Commit();

        return new BaseResponse();
    }
}
