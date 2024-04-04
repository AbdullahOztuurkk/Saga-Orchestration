using Application.Services.Abstract;
using CoreLib.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

public class StockController : MainApiController
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        this._stockService = stockService;
    }

    [HttpGet]
    public async Task<BaseResponse> GetAll()
    {
        var result = await _stockService.GetAll();
        return result;
    }
}

