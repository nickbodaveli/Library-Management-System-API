using Library.Application.Common.Dtos;
using Library.Application.Common.Models;
using Library.Application.Data;
using Library.Application.Library.Books.Queries.GetPaginatedBooks;
using Library.Domain.ValueObjects.Book;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Library.API.Data
{
    public class CachedBookReadRepository : IBookReadRepository
    {
        private readonly IBookReadRepository _decoratedRepository;
        private readonly IDistributedCache _cache;

        public CachedBookReadRepository(IBookReadRepository decoratedRepository, IDistributedCache cache) //
        {
            _decoratedRepository = decoratedRepository;
            _cache = cache;
        }

        public async Task<Dictionary<Guid, BookReadDto>> GetBooksDetailsByIdsAsync(
            IEnumerable<Guid> bookIds,
            CancellationToken cancellationToken)
        {
            var sortedIds = string.Join(",", bookIds.OrderBy(id => id));

            var keySuffix = sortedIds.GetHashCode().ToString();
            var key = $"BooksDetailsByList_{keySuffix}";

            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                var json = Encoding.UTF8.GetString(cachedBytes);
                return JsonSerializer.Deserialize<Dictionary<Guid, BookReadDto>>(json)!;
            }

            var result = await _decoratedRepository.GetBooksDetailsByIdsAsync(bookIds, cancellationToken);

            if (result != null && result.Any())
            {
                var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
                await _cache.SetAsync(key, bytesToCache, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                }, cancellationToken);
            }

            return result;
        }

        public async Task<BookReadDto?> GetBookDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var key = $"BookDetails_{id}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                return JsonSerializer.Deserialize<BookReadDto>(Encoding.UTF8.GetString(cachedBytes));
            }

            var result = await _decoratedRepository.GetBookDtoByIdAsync(id, cancellationToken);

            if (result != null)
            {
                var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
                await _cache.SetAsync(key, bytesToCache,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) },
                    cancellationToken);
            }

            return result;
        }

        public async Task<GetPaginatedBooksQueryResult> GetPaginatedBooksAsync(
               int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var key = $"BooksList_{pageIndex}_{pageSize}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<GetPaginatedBooksQueryResult>(Encoding.UTF8.GetString(cachedBytes), options)!;
            }

            var result = await _decoratedRepository.GetPaginatedBooksAsync(pageIndex, pageSize, cancellationToken);

            var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
            await _cache.SetAsync(key, bytesToCache,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
                cancellationToken);

            return result;
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
        {
            return await _decoratedRepository.ExistsByIsbnAsync(isbn, cancellationToken);
        }

        public async Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            const int pageSize = 10;
            const int pagesToClear = 5; 

            for (int i = 0; i < pagesToClear; i++)
            {
                var key = $"BooksList_{i}_{pageSize}";
                tasks.Add(_cache.RemoveAsync(key, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        public async Task AddAsync(BookReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.AddAsync(readModel, cancellationToken);

            await InvalidatePaginatedListCachesAsync(cancellationToken);
        }

        public async Task UpdateAsync(BookReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.UpdateAsync(readModel, cancellationToken);

            var bookDetailsKey = $"BookDetails_{readModel.Id}";
            await _cache.RemoveAsync(bookDetailsKey, cancellationToken);

            await InvalidatePaginatedListCachesAsync(cancellationToken);
        }

        public Task UpdateAvailableCountAsync(Guid bookId, int changeAmount, CancellationToken cancellationToken)
        {
            return _decoratedRepository.UpdateAvailableCountAsync(bookId, changeAmount, cancellationToken);
        }
    }
}
