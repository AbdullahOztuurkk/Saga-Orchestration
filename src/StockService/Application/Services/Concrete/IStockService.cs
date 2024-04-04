using Application.Services.Abstract;
using CoreLib.Entity.Enums;
using CoreLib.ResponseModel;
using Domain.Concrete;
using Domain.Dtos;
using SharedLib.ValueObject;

namespace Application.Services.Concrete;
public class StockService : BaseService , IStockService
{
    public async Task<BaseResponse> CheckStock(List<OrderItemMessage> OrderItems)
    {
        var response = new BaseResponse();
        var stockResult = new List<bool>();

        for(int index = 0; index < OrderItems.Count; index++)
        {
            var stocks = await Db.GetDefaultRepo<Stock>().GetManyAsync(x => x.ProductId == OrderItems[index].ProductId && x.Count > OrderItems[index].Count);
            stockResult.Add(stocks != null && stocks.Count != 0);
        }

        if (!stockResult.All(item => item.Equals(true)))
            response.IsSuccess = false;

        return response;
    }

    public async Task<BaseResponse> GetAll()
    {
        var response = new BaseResponse();

        var stocks = await Db.GetDefaultRepo<Stock>().GetAllAsync();

        response.Data = stocks;

        return response;
    }

    public async Task<BaseResponse<Stock>> GetByProductId(int productId)
    {
        var stock = await Db.GetDefaultRepo<Stock>().GetAsync(x => x.ProductId == productId && x.Status == StatusType.ACTIVE);
        if (stock == null)
            return new BaseResponse<Stock> { IsSuccess = false };

        return new BaseResponse<Stock> { Data = stock };
    }

    public async Task<BaseResponse> Update(StockUpdateRequestDto request)
    {
        var stock = await Db.GetDefaultRepo<Stock>().GetAsync(x => x.ProductId == request.ProductId && x.Status == StatusType.ACTIVE);
        if (stock == null)
            return new BaseResponse { IsSuccess = false };

        stock.Count = request.Count;

        await Db.GetDefaultRepo<Stock>().SaveChanges();
        Db.Commit();

        return new BaseResponse();
    }
}
