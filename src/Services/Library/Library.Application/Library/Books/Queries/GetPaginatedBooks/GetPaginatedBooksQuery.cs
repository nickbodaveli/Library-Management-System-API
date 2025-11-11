using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Books.Queries.GetPaginatedBooks
{
    public record GetPaginatedBooksQuery(PaginationRequest PaginationRequest) : IQuery<GetPaginatedBooksQueryResult>;

    public record GetPaginatedBooksQueryResult(PaginatedResult<BookReadDto> Book);
}
