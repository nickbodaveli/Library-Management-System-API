using Library.Domain.Models.Loan;
using Library.Domain.ValueObjects.Loan;

namespace Library.Application.Data
{
    public interface ILoanRepository : IBaseRepository<Loan, LoanId>
    {
        Task<Loan> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Loan> GetActiveLoanByBookIdAndMemberIdAsync(
            Guid bookId,
            Guid memberId,
            CancellationToken cancellationToken = default);
        Task<int> GetActiveLoanCountByMemberAsync(
            Guid memberId,
            CancellationToken cancellationToken = default);
        Task UpdateAsync(Loan loan, CancellationToken cancellationToken = default);
    }
}
