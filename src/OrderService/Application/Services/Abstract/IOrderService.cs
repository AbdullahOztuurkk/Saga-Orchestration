using CoreLib.ResponseModel;
using Domain.Dtos;

namespace Application.Services.Abstract;
public interface IOrderService
{
    Task<BaseResponse> Update(OrderUpdateRequestDto request);
    Task<BaseResponse> CreateOrder(OrderCreateRequestDto request);
    Task<BaseResponse> GetByOrderId(long OrderId);
}
