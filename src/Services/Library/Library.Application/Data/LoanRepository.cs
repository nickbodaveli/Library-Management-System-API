using Library.Domain.Models.Loan;
using Library.Domain.ValueObjects.Book;
using Library.Domain.ValueObjects.Loan;
using Library.Domain.ValueObjects.Member;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Data
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IApplicationDbContext _context; 

        public LoanRepository(IApplicationDbContext context) => _context = context;

        public Task AddAsync(Loan loan, CancellationToken cancellationToken)
        {
            _context.Loans.Add(loan);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Loan loan, CancellationToken cancellationToken = default)
        {
            _context.Loans.Update(loan);
            return Task.CompletedTask;
        }

        public async Task<Loan> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .FirstOrDefaultAsync(l => l.Id == LoanId.Of(id), cancellationToken);
        }

        public async Task<int> GetActiveLoanCountByMemberAsync(
            Guid memberId, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .CountAsync(l => l.MemberId == MemberId.Of(memberId) && 
                               l.ReturnDate == null, cancellationToken);
        }

        public async Task<Loan?> GetActiveLoanByBookIdAndMemberIdAsync(
            Guid bookId, Guid memberId, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .FirstOrDefaultAsync(l => l.BookId == BookId.Of(bookId) && 
                                       l.MemberId == MemberId.Of(memberId) && 
                                       l.ReturnDate == null, 
                                       cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);
    }
}
