using BookStore.Common.Api;
using BookStore.Common.Constants;
using BookStore.Data.Contracts;
using BookStore.Entities;
using BookStore.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Services;

public class BookService : IBookService
{
    private IRepository<Book> bookRepository;
    private readonly ILogger<Book> logger;
    public BookService(IRepository<Book> bookRepository, ILogger<Book> logger)
    {
        this.bookRepository = bookRepository;
        this.logger = logger;
    }
    public async Task<ApiResult<List<BookListDto>>> FilterBooks(QueryBookDto bookDto, CancellationToken cancellationToken)
    {
        try
        {
            var booksQuery = bookRepository.TableNoTracking.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(bookDto.Title))
            {
                booksQuery = booksQuery.Where(p => p.Title.Contains(bookDto.Title));
            }

            if (bookDto.CategoryId != null)
            {
                booksQuery = booksQuery.Where(p => p.CategoryId == bookDto.CategoryId);
            }

            var count = (int)Math.Ceiling(booksQuery.Count() / (double)bookDto.TakeEntity);

            var pager = Pager.Build(count, bookDto.PageId, bookDto.TakeEntity);

            var books = booksQuery.Paging(pager).Select(p => new BookListDto
            {
                Id = p.Id,
                Title = p.Title,
                Category = p.Category.Title,
                Picture = p.Picture ?? "",
                Price = p.Price.ToString(),

            }).ToList();

            return new ApiResult<List<BookListDto>>(true, ApiResultStatusCode.Success, books, CommonStrings.SuccessMessage);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in FilterBooks Api! " + ex.Message);
            return new ApiResult<List<BookListDto>>(false, ApiResultStatusCode.ServerError, null, CommonStrings.ErrorMesssage);
        }
    }
    public void Dispose()
    {
        bookRepository?.Dispose();
    }
}

