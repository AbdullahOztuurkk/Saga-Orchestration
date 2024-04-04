using Application.Services.Abstract;
using CoreLib.ResponseModel;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

public class OrderController : MainApiController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        this._orderService = orderService;
    }

    [HttpPost]
    public async Task<BaseResponse> CreateOrder(OrderCreateRequestDto request)
    {
        var result = await _orderService.CreateOrder(request);
        return result;
    }
}
