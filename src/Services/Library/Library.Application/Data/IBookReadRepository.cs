using Library.Application.Common.Dtos;
using Library.Application.Library.Books.Queries.GetPaginatedBooks;

namespace Library.Application.Data
{
    public interface IBookReadRepository
    {
        Task<BookReadDto?> GetBookDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Dictionary<Guid, BookReadDto>> GetBooksDetailsByIdsAsync(
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken);
        Task<GetPaginatedBooksQueryResult> GetPaginatedBooksAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken);

        Task<bool> ExistsByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
        Task AddAsync(BookReadDto readModel, CancellationToken cancellationToken);
        Task UpdateAsync(BookReadDto readModel, CancellationToken cancellationToken);
        Task UpdateAvailableCountAsync(Guid bookId, int changeAmount, CancellationToken cancellationToken);
        Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken);
    }
}
