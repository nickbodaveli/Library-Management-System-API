using Library.Domain.Models.Book;
using Library.Domain.ValueObjects.Book;

namespace Library.Application.Data
{
    public interface IBookRepository : IBaseRepository<Book, BookId>
    {
        Task<Book> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
        Task BulkInsertAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default);
    }
}
