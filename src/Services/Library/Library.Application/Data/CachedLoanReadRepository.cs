using Library.Application.Common.Dtos;
using Library.Application.Library.Loans.Queries.GetOverDueLoans;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Library.Application.Data
{
    public class CachedLoanReadRepository : ILoanReadRepository
    {
        private readonly ILoanReadRepository _decoratedRepository;
        private readonly IDistributedCache _cache;

        private const int ListPageSize = 10;
        private const int ListPagesToClear = 5;

        public CachedLoanReadRepository(ILoanReadRepository decoratedRepository, IDistributedCache cache)
        {
            _decoratedRepository = decoratedRepository;
            _cache = cache;
        }

        public async Task InvalidateActiveLoansCacheAsync(Guid memberId, CancellationToken cancellationToken)
        {
            var key = $"MemberActiveLoans_{memberId}";
            await _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task InvalidatePaginatedOverdueListCachesAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < ListPagesToClear; i++)
            {
                var key = $"OverdueLoansList_{i}_{ListPageSize}";
                tasks.Add(_cache.RemoveAsync(key, cancellationToken));
            }
            await Task.WhenAll(tasks);
        }

        public async Task<LoanReadDto?> GetLoanDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var key = $"LoanDetails_{id}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                return JsonSerializer.Deserialize<LoanReadDto>(Encoding.UTF8.GetString(cachedBytes));
            }

            var result = await _decoratedRepository.GetLoanDtoByIdAsync(id, cancellationToken);

            if (result != null)
            {
                var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
                await _cache.SetAsync(key, bytesToCache, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                }, cancellationToken);
            }

            return result;
        }

        public async Task<List<LoanReadDto>> GetActiveLoansByMemberIdAsync(Guid memberId, CancellationToken cancellationToken)
        {
            var key = $"MemberActiveLoans_{memberId}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                var json = Encoding.UTF8.GetString(cachedBytes);
                return JsonSerializer.Deserialize<List<LoanReadDto>>(json)!;
            }

            var result = await _decoratedRepository.GetActiveLoansByMemberIdAsync(memberId, cancellationToken);

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

        public async Task<GetPaginatedOverDueLoansQueryResult> GetOverdueLoansAsync(
            int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var key = $"OverdueLoansList_{pageIndex}_{pageSize}";
            var cachedBytes = await _cache.GetAsync(key, cancellationToken);

            if (cachedBytes != null)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<GetPaginatedOverDueLoansQueryResult>(Encoding.UTF8.GetString(cachedBytes), options)!;
            }

            var result = await _decoratedRepository.GetOverdueLoansAsync(pageIndex, pageSize, cancellationToken);

            var bytesToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
            await _cache.SetAsync(key, bytesToCache, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) 
            }, cancellationToken);

            return result;
        }

        public async Task AddAsync(LoanReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.AddAsync(readModel, cancellationToken);

            await InvalidateActiveLoansCacheAsync(readModel.MemberId, cancellationToken);

            if (readModel.DueDate.Date < DateTime.UtcNow.Date)
            {
                await InvalidatePaginatedOverdueListCachesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync(LoanReadDto readModel, CancellationToken cancellationToken)
        {
            await _decoratedRepository.UpdateAsync(readModel, cancellationToken);

            var loanDetailsKey = $"LoanDetails_{readModel.Id}";
            await _cache.RemoveAsync(loanDetailsKey, cancellationToken);

            await InvalidateActiveLoansCacheAsync(readModel.MemberId, cancellationToken);

            await InvalidatePaginatedOverdueListCachesAsync(cancellationToken);
        }
    }
}
