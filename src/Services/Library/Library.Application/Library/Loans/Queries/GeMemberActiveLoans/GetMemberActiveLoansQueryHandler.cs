using Library.Application.Common.Dtos;
using Library.Application.Data;
using MediatR;

namespace Library.Application.Library.Loans.Queries.GeMemberActiveLoans
{
    public class GetMemberActiveLoansHandler(ILoanReadRepository _loanReadRepository, IBookReadRepository _bookReadRepository) : IRequestHandler<GetMemberActiveLoansQuery, List<MemberActiveLoanDto>>
    {
        public async Task<List<MemberActiveLoanDto>> Handle(GetMemberActiveLoansQuery request, CancellationToken cancellationToken)
        {
            var activeLoans = await _loanReadRepository.GetActiveLoansByMemberIdAsync(request.MemberId, cancellationToken);

            if (!activeLoans.Any())
            {
                return new List<MemberActiveLoanDto>();
            }

            var bookIds = activeLoans.Select(l => l.BookId).Distinct();

            var booksDictionary = await _bookReadRepository.GetBooksDetailsByIdsAsync(
                bookIds,
                cancellationToken);

            var result = activeLoans.Select(loan =>
            {
                if (booksDictionary.TryGetValue(loan.BookId, out var bookDetails))
                {
                    return new MemberActiveLoanDto(
                        loan.Id,
                        bookDetails.Title,
                        bookDetails.Author,
                        loan.LoanDate,
                        loan.DueDate
                    );
                }
                return null;
            })
            .Where(dto => dto != null) 
            .ToList();

            return result!; 
        }
    }
}
