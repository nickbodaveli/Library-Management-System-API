using Library.Application.Common.Dtos;
using Library.Application.Library.Members.Queries.GetPaginatedMembers;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Library.Application.Data
{
    public class CachedMemberReadRepository : IMemberReadRepository
    {
        private readonly IMemberReadRepository _decoratedRepository;
        private readonly IDistributedCache _cache;

        public CachedMemberReadRepository(IMemberReadRepository decoratedRepository, IDistributedCache cache)
        {
            _decoratedRepository = decoratedRepository;
            _cache = cache;
        }

        public async Task<MemberReadDto?> GetMemberDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var key = $"MemberDetails_{id}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                return JsonSerializer.Deserialize<MemberReadDto>(Encoding.UTF8.GetString(cachedBytes));
            }

            var result = await _decoratedRepository.GetMemberDtoByIdAsync(id, cancellationToken);

            if (result != null)
            {
                var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
                await _cache.SetAsync(
                     key,
                     bytesToCache,
                     new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) },
                     cancellationToken
                 );
            }

            return result;
        }

        public async Task<GetPaginatedMemberQueryResult> GetPaginatedMembersAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var key = $"MembersList_{pageIndex}_{pageSize}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<GetPaginatedMemberQueryResult>(Encoding.UTF8.GetString(cachedBytes), options)!;
            }

            var result = await _decoratedRepository.GetPaginatedMembersAsync(pageIndex, pageSize, cancellationToken);

            var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
            await _cache.SetAsync(
                 key,
                 bytesToCache,
                 new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
                 cancellationToken
             );

            return result;
        }

        public async Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            const int pageSize = 10;
            const int pagesToClear = 5;

            for (int i = 0; i < pagesToClear; i++)
            {
                var key = $"MembersList_{i}_{pageSize}";
                tasks.Add(_cache.RemoveAsync(key, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        public async Task AddAsync(MemberReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.AddAsync(readModel, cancellationToken);

            await InvalidatePaginatedListCachesAsync(cancellationToken);
        }

        public async Task UpdateAsync(MemberReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.UpdateAsync(readModel, cancellationToken);

            var memberDetailsKey = $"MemberDetails_{readModel.Id}";
            await _cache.RemoveAsync(memberDetailsKey, cancellationToken);

            await InvalidatePaginatedListCachesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _decoratedRepository.ExistsByEmailAsync(email, cancellationToken);
        }
    }
}
