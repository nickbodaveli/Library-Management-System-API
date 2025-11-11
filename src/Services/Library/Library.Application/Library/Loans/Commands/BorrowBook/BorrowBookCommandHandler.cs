using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Domain.Models.Loan;
using Library.Domain.ValueObjects.Book;
using Library.Domain.ValueObjects.Loan;
using Library.Domain.ValueObjects.Member;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Library.Loans.Commands.BorrowBook
{
    public class BorrowBookCommandHandler(IBookRepository _bookRepository, IMemberRepository _memberRepository, ILoanRepository _loanRepository, ICacheInvalidationService _cacheInvalidationService) : IRequestHandler<BorrowBookCommand, BorrowBookResult>
    {
        public async Task<BorrowBookResult> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            const int MaxRetries = 3;

            for (int retry = 0; retry < MaxRetries; retry++)
            {
                try
                {
                    var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
                    var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

                    if (book is null) throw new BookNotFoundException($"Book with ID {request.BookId} not found.");
                    if (member is null || !member.IsActive) throw new MemberInActiveException($"Member is inactive or not found.");

                    if (await _loanRepository.GetActiveLoanCountByMemberAsync(request.MemberId, cancellationToken) >= 5)
                    {
                        throw new MemberMaximumLimitException($"Member has reached the maximum limit of 5 active loans.");
                    }

                    var newLoanId = LoanId.Of(Guid.NewGuid());
                    book.Borrow(newLoanId, MemberId.Of(request.MemberId));

                    var loan = new Loan(newLoanId, BookId.Of(request.BookId), MemberId.Of(request.MemberId), DateTime.UtcNow.AddDays(request.LoanPeriodDays));

                    await _bookRepository.UpdateAsync(book, cancellationToken);
                    await _loanRepository.AddAsync(loan, cancellationToken);

                    await _loanRepository.SaveChangesAsync(cancellationToken);

                    var memberLoansKey = $"MemberActiveLoans_{request.MemberId}";
                    await _cacheInvalidationService.InvalidateAsync(memberLoansKey, cancellationToken);

                    var bookDetailsKey = $"BookDetails_{request.BookId}";
                    await _cacheInvalidationService.InvalidateAsync(bookDetailsKey, cancellationToken);

                    await _cacheInvalidationService.InvalidateByPatternAsync("PaginatedBooksList_", cancellationToken);


                    return new BorrowBookResult(loan.Id.Value);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (retry >= MaxRetries - 1)
                    {
                        throw new Exception("Failed to borrow book due to concurrent updates. Please try again.");
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(50 * (retry + 1)), cancellationToken);
                }
            }

            throw new InvalidOperationException("OCC retry loop finished unexpectedly.");
        }
    }
}
