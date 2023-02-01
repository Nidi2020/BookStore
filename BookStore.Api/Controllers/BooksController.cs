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
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        public async Task<ApiResult<List<BookListDto>>> Get([FromQuery] QueryBookDto bookDto, CancellationToken cancellationToken)
        {
            var userId = User.Identity?.GetUserId<int>() ?? null;
            if (userId == null)
                return new ApiResult<List<BookListDto>>(false, ApiResultStatusCode.UnAuthorized, null, CommonStrings.UnAuthorizedMessage);

            return await bookService.FilterBooks(bookDto, cancellationToken);
        }
    }
}
