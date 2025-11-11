using Library.Application.Common.Dtos;
using Library.Application.Library.Loans.Queries.GetOverDueLoans;

namespace Library.Application.Data
{
    public interface ILoanReadRepository
    {
        Task<LoanReadDto?> GetLoanDtoByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<LoanReadDto>> GetActiveLoansByMemberIdAsync(Guid memberId, CancellationToken cancellationToken);
        Task<GetPaginatedOverDueLoansQueryResult> GetOverdueLoansAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken);

        Task AddAsync(LoanReadDto readModel, CancellationToken cancellationToken);
        Task UpdateAsync(LoanReadDto readModel, CancellationToken cancellationToken);
        Task InvalidateActiveLoansCacheAsync(Guid memberId, CancellationToken cancellationToken);
        Task InvalidatePaginatedOverdueListCachesAsync(CancellationToken cancellationToken);
    }
}
