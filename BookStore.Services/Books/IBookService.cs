using BookStore.Common.Api;
using BookStore.Models.Dtos;

namespace BookStore.Services
{
    public interface IBookService : IDisposable
    {
        /// <summary>
        /// get all list books with paging
        /// </summary>
        /// <param name="bookDto">filter parameters</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ApiResult<List<BookListDto>>> FilterBooks(QueryBookDto bookDto, CancellationToken cancellationToken);
    }
}
