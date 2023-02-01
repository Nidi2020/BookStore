using BookStore.Common.Api;
using BookStore.Entities;
using BookStore.Models.Dtos;

namespace BookStore.Services;

public interface IOrderService : IDisposable
{
    Task<ApiResult> AddOrder(int userId, AddOrderDto orderDto, CancellationToken cancellationToken);

    Task AddOrderDetail(Order order, List<Book> books, User user, CancellationToken cancellationToken);

    Task<ApiResult<List<OrderListDto>>> FilterOrders(int userId, QueryOrderDto orderDto, CancellationToken cancellationToken);
}

