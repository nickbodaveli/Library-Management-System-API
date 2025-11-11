using Library.Domain.Models.Book;
using Library.Domain.ValueObjects.Book;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IApplicationDbContext _context;

        public BookRepository(IApplicationDbContext context) => _context = context;

        public Task AddAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Add(book);
            return Task.CompletedTask; 
        }

        public async Task<Book> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == BookId.Of(id), cancellationToken);
        }

        public async Task<bool> ExistsByISBNAsync(string isbn, CancellationToken cancellationToken = default) =>
            await _context.Books.AnyAsync(b => b.ISBN == isbn, cancellationToken);

        public Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Update(book);
            return Task.CompletedTask;
        }

        // BulkInsertAsync
        public async Task BulkInsertAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default)
        {
            await _context.Books.AddRangeAsync(books, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
