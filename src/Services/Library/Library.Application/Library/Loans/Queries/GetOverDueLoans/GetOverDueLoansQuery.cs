using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Loans.Queries.GetOverDueLoans
{
    public record GetPaginatedOverDueLoansQuery(PaginationRequest PaginationRequest) : IQuery<GetPaginatedOverDueLoansQueryResult>;

    public record GetPaginatedOverDueLoansQueryResult(PaginatedResult<LoanReadDto> Loans);
}
