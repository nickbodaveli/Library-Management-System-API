using Library.Application.Data;
using MediatR;

namespace Library.Application.Library.Loans.Queries.GetOverDueLoans
{
    public class GetOverdueLoansQueryHandler(ILoanReadRepository _loanReadRepository) : IRequestHandler<GetPaginatedOverDueLoansQuery, GetPaginatedOverDueLoansQueryResult>
    {
        public Task<GetPaginatedOverDueLoansQueryResult> Handle(
            GetPaginatedOverDueLoansQuery request,
            CancellationToken cancellationToken)
        {
            return _loanReadRepository.GetOverdueLoansAsync(
                request.PaginationRequest.PageIndex,
                request.PaginationRequest.PageSize,
                cancellationToken
            );
        }
    }
}
