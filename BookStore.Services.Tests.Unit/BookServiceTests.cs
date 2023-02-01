using BookStore.Models.Dtos;
using BookStore.Services.Tests.Unit.ClassFixtures;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Tests.Unit
{
    public class BookServiceTests : IClassFixture<BookFixture>
    {
        private BookService _bookService;
        public BookServiceTests(BookFixture bookFixture)
        {
            _bookService = bookFixture.bookService;
        }

        [Fact]
        public async Task WhenFilterBooksCallShouldReturnListOfBooks()
        {
            //arrange
            var queryOrderDto = new QueryBookDto()
            {
                PageId = 1,
            };

            //act
            var result = await _bookService.FilterBooks(queryOrderDto, System.Threading.CancellationToken.None);

            //assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
    }
}
