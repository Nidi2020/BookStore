using BookStore.Data.Contracts;
using BookStore.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace BookStore.Services.Tests.Unit.ClassFixtures
{
    public class BookFixture : IDisposable
    {
        public BookService bookService;
        public Mock<IRepository<Book>> bookRepository;
        public Mock<ILogger<Book>> logger;
        public BookFixture()
        {
            bookRepository = new Mock<IRepository<Book>>();
            logger = new Mock<ILogger<Book>>();

            bookService = new BookService(bookRepository.Object, logger.Object);
        }
        public void Dispose()
        {
            bookRepository.Object.Dispose();
            bookService.Dispose();
        }
    }
}
