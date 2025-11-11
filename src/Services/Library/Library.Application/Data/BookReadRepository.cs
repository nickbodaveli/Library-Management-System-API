using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;
using Library.Application.Library.Books.Queries.GetPaginatedBooks;
using MongoDB.Driver;

namespace Library.Application.Data
{
    public class BookReadRepository : IBookReadRepository
    {
        private readonly IMongoCollection<BookReadDto> _books;

        public BookReadRepository(IMongoDatabase database)
        {
            _books = database.GetCollection<BookReadDto>("BooksRead");
        }

        public async Task<Dictionary<Guid, BookReadDto>> GetBooksDetailsByIdsAsync(
        IEnumerable<Guid> bookIds,
        CancellationToken cancellationToken)
        {
            var filter = Builders<BookReadDto>.Filter.In(b => b.Id, bookIds);

            var booksList = await _books.Find(filter).ToListAsync(cancellationToken);

            return booksList.ToDictionary(b => b.Id, b => b);
        }

        public async Task<BookReadDto?> GetBookDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<BookReadDto>.Filter.Eq(x => x.Id, id);
            return await _books.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetPaginatedBooksQueryResult> GetPaginatedBooksAsync(
            int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var filter = Builders<BookReadDto>.Filter.Empty;

            var totalCount = await _books.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var dtos = await _books.Find(filter)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new GetPaginatedBooksQueryResult(
                new PaginatedResult<BookReadDto>(
                    pageIndex,
                    pageSize,
                    (int)totalCount,
                    dtos));
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
        {
            var filter = Builders<BookReadDto>.Filter.Eq(x => x.ISBN, isbn);
            var count = await _books.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            return count > 0;
        }

        public async Task AddAsync(BookReadDto readModel, CancellationToken cancellationToken)
        {
            await _books.InsertOneAsync(readModel, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(BookReadDto readModel, CancellationToken cancellationToken)
        {
            var filter = Builders<BookReadDto>.Filter.Eq(x => x.Id, readModel.Id);
            await _books.ReplaceOneAsync(filter, readModel, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public async Task UpdateAvailableCountAsync(Guid bookId, int changeAmount, CancellationToken cancellationToken)
        {
            var filter = Builders<BookReadDto>.Filter.Eq(x => x.Id, bookId);
            var update = Builders<BookReadDto>.Update.Inc(x => x.AvailableCopies, changeAmount);

            await _books.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }

        public Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
