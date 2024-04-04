using CoreLib.ResponseModel;
using Domain.Concrete;
using Domain.Dtos;
using SharedLib.ValueObject;

namespace Application.Services.Abstract;
public interface IStockService
{
    Task<BaseResponse> Update(StockUpdateRequestDto request);
    Task<BaseResponse<Stock>> GetByProductId(int productId);
    Task<BaseResponse> CheckStock(List<OrderItemMessage> OrderItems);
    Task<BaseResponse> GetAll();
}
