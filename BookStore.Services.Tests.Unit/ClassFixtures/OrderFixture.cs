using BookStore.Data.Contracts;
using BookStore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace BookStore.Services.Tests.Unit.ClassFixtures
{
    public class OrderFixture : IDisposable
    {
        public OrderService orderService;
        public Mock<IRepository<Order>> orderRepository;
        public Mock<IRepository<OrderDetail>> orderDetailRepository;
        public Mock<IRepository<Book>> bookRepository;
        public Mock<IUserService> userService;
        public Mock<ILogger<User>> logger;
        public Mock<UserManager<User>> userManager;

        public OrderFixture()
        {
            orderRepository = new Mock<IRepository<Order>>();
            orderDetailRepository = new Mock<IRepository<OrderDetail>>();
            bookRepository = new Mock<IRepository<Book>>();
            userService = new Mock<IUserService>();
            logger = new Mock<ILogger<User>>();
            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            orderService = new OrderService(orderRepository.Object, orderDetailRepository.Object, bookRepository.Object, userService.Object, logger.Object, userManager.Object);
        }

        public void Dispose()
        {
            orderRepository.Object.Dispose();
            orderDetailRepository.Object.Dispose();
            bookRepository.Object.Dispose();
            userService.Object.Dispose();
            userManager.Object.Dispose();
            orderService.Dispose();
        }
    }
}
