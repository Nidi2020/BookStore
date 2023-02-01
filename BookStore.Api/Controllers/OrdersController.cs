using BookStore.Common.Api;
using BookStore.Common.Constants;
using BookStore.Common.Utilities;
using BookStore.Models.Dtos;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("add")]
        public async Task<ApiResult> AddOrder(AddOrderDto addOrderDto, CancellationToken cancellationToken)
        {
            var userId = User.Identity?.GetUserId<int>() ?? null;
            if (userId == null)
                return new ApiResult(false, ApiResultStatusCode.UnAuthorized, CommonStrings.UnAuthorizedMessage);

            return await orderService.AddOrder((int)userId, addOrderDto, cancellationToken);
        }

        [HttpGet]
        public async Task<ApiResult<List<OrderListDto>>> Get([FromQuery] QueryOrderDto orderDto, CancellationToken cancellationToken)
        {
            var userId = User.Identity?.GetUserId<int>() ?? null;
            if (userId == null)
                return new ApiResult<List<OrderListDto>>(false, ApiResultStatusCode.UnAuthorized, null, CommonStrings.UnAuthorizedMessage);

            return await orderService.FilterOrders((int)userId, orderDto, cancellationToken);
        }
    }
}
