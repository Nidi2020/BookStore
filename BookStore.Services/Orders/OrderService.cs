using BookStore.Common.Api;
using BookStore.Common.Constants;
using BookStore.Data.Contracts;
using BookStore.Entities;
using BookStore.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookStore.Services;

public class OrderService : IOrderService
{
    public IRepository<Order> orderRepository;
    public IRepository<OrderDetail> orderDetailRepository;
    public IRepository<Book> bookRepository;
    public IUserService userService;
    public ILogger<User> logger;
    public UserManager<User> userManager;
    public OrderService(IRepository<Order> orderRepository, IRepository<OrderDetail> orderDetailRepository, IRepository<Book> bookRepository,
        IUserService userService, ILogger<User> logger, UserManager<User> userManager)
    {
        this.orderRepository = orderRepository;
        this.orderDetailRepository = orderDetailRepository;
        this.bookRepository = bookRepository;
        this.userService = userService;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task<ApiResult> AddOrder(int userId, AddOrderDto orderDto, CancellationToken cancellationToken)
    {
        try
        {
            if (orderDto.BookIds.Count == 0)
                return new ApiResult(false, ApiResultStatusCode.ListEmpty, CommonStrings.BadRequestMessage);


            var books = bookRepository.TableNoTracking.Where(p => orderDto.BookIds.Contains(p.Id) && !p.IsDeleted).ToList();
            int totalPrice = books.Sum(p => p.Price);
            var user = await userService.GetUserById(userId);
            var order = new Order
            {
                UserId = userId,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                IsVerify = true,
                TotalPrice = totalPrice,
            };

            await orderRepository.AddAsync(order, cancellationToken);

            await AddOrderDetail(order, books, user, cancellationToken);

            order.TotalScore = orderDetailRepository.TableNoTracking.Where(p => p.OrderId == order.Id).Sum(s => (int)s.Score);
            await orderRepository.UpdateAsync(order, cancellationToken);

            return new ApiResult(true, ApiResultStatusCode.Success, CommonStrings.SuccessMessage);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in AddOrder Api! " + ex.Message);
            return new ApiResult(false, ApiResultStatusCode.ServerError, CommonStrings.ErrorMesssage);
        }
    }

    public async Task AddOrderDetail(Order order, List<Book> books, User user, CancellationToken cancellationToken)
    {
        foreach (var book in books)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                BookId = book.Id,
                Count = 1,
                Price = book.Price,
                Score = userService.GetUserScore(user?.Age ?? 0),
                CreateAt = order.CreateAt,
                CreateBy = order.CreateBy
            };
            await orderDetailRepository.AddAsync(orderDetail, cancellationToken);
        }
    }
    public async Task<ApiResult<List<OrderListDto>>> FilterOrders(int userId, QueryOrderDto orderDto, CancellationToken cancellationToken)
    {
        try
        {
            var ordersQuery = orderRepository.Table.AsQueryable();
            var user = await userService.GetUserById(userId);

            if (await userManager.IsInRoleAsync(user, "User"))
            {
                ordersQuery = ordersQuery.Where(p => p.UserId == user.Id);
            }
            if (!string.IsNullOrEmpty(orderDto.Title))
            {
                ordersQuery = ordersQuery.Where(p => p.OrderDetails.Where(s => s.Book.Title.Contains(orderDto.Title)).Count() > 0);
            }
            var count = (int)Math.Ceiling(ordersQuery.Count() / (double)orderDto.TakeEntity);

            var pager = Pager.Build(count, orderDto.PageId, orderDto.TakeEntity);

            var orders = ordersQuery.Paging(pager).Select(p => new OrderListDto
            {
                Id = p.Id,
                Name = p.User.Name,
                Family = p.User.Family,
                Books = orderDetailRepository.TableNoTracking.Where(d => d.OrderId == p.Id).Select(b => new BookListDto
                {
                    Id = b.BookId,
                    Title = b.Book.Title,
                    Category = b.Book.Category.Title,
                    Picture = b.Book.Picture ?? "",
                    Price = b.Book.Price.ToString(),

                }).ToList(),

                Score = p.TotalScore

            }).ToList();

            return new ApiResult<List<OrderListDto>>(true, ApiResultStatusCode.Success, orders, CommonStrings.SuccessMessage);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in FilterOrders api! " + ex.Message);
            return new ApiResult<List<OrderListDto>>(false, ApiResultStatusCode.ServerError, null, CommonStrings.ErrorMesssage);
        }
    }
    public void Dispose()
    {
        orderRepository?.Dispose();
        orderDetailRepository?.Dispose();
        bookRepository?.Dispose();
    }
}

