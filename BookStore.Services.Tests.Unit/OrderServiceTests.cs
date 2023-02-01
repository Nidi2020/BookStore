using BookStore.Data.Contracts;
using BookStore.Entities;
using BookStore.Models.Dtos;
using BookStore.Services.Tests.Unit.ClassFixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;

namespace BookStore.Services.Tests.Unit
{
    public class OrderServiceTests : IClassFixture<OrderFixture>
    {
        private OrderService _orderService;
        private Mock<IRepository<Book>> _bookRepository;
        private Mock<IRepository<OrderDetail>> _orderDetailRepository;
        public OrderServiceTests(OrderFixture orderFixture)
        {
            _orderService = orderFixture.orderService;
            _bookRepository = orderFixture.bookRepository;
            _orderDetailRepository = orderFixture.orderDetailRepository;
        }

        [Fact]
        public async Task WhenAddAOrder_ShouldReturnSuccessed()
        {
            //arrange
            var command = SomeCreateOrder();
            const int userId = 3;

            var books = new List<Book>()
            {
                new Book() { Id= 1 , Title = "x1", Price=100, Author="jack", Code = "10"},
                new Book() { Id= 2 , Title = "x2", Price=110, Author="jack", Code = "11"},
            };
            var myDbSet = GetQueryableMockDbSet(books);

            _bookRepository.Setup(x => x.TableNoTracking).Returns(myDbSet);

            //act
            var expected = await _orderService.AddOrder(userId, command, System.Threading.CancellationToken.None);

            //assert
            expected.IsSuccess.Should().Be(true);
        }

        private static AddOrderDto SomeCreateOrder()
        {
            var filler = new Filler<AddOrderDto>();
            filler.Setup().OnProperty(x => x.BookIds).Use(new List<int>() { 1, 2 });
            return filler.Create();
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }

        [Fact]
        public async Task WhenAddOrderDetail_ShouldReturnSuccessed()
        {
            //arrange
            var user = new User() { Id = 1, Name = "neda" };
            var order = new Order() { Id = 1, TotalPrice = 200, IsVerify = true };
            var books = new List<Book>()
            {
                new Book() { Id= 1 , Title = "x1", Price=100, Author="jack", Code = "10"},
                new Book() { Id= 2 , Title = "x2", Price=110, Author="jack", Code = "11"},
            };

            //act
            await _orderService.AddOrderDetail(order, books, user, System.Threading.CancellationToken.None);

            //assert
            _orderDetailRepository.VerifyAll();
        }

        [Fact]
        public async Task WhenFilterOrdersCallShouldReturnListOfOrders()
        {
            //arrange
            const int userId = 3;
            var queryOrderDto = new QueryOrderDto()
            {
                PageId = 1,
            };

            //act
            var result = await _orderService.FilterOrders(userId, queryOrderDto, System.Threading.CancellationToken.None);

            //assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
    }
}
