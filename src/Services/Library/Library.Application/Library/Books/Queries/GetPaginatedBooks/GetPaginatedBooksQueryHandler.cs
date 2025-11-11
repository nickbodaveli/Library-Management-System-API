using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;
using Library.Application.Common.Models;
using Library.Application.Data;

namespace Library.Application.Library.Books.Queries.GetPaginatedBooks
{
    public class GetPaginatedBooksQueryHandler(IBookReadRepository _bookReadRepository) : IQueryHandler<GetPaginatedBooksQuery, GetPaginatedBooksQueryResult>
    {
        public async Task<GetPaginatedBooksQueryResult> Handle(GetPaginatedBooksQuery request, CancellationToken cancellationToken)
        {
            var result = await _bookReadRepository.GetPaginatedBooksAsync(
                request.PaginationRequest.PageIndex,
                request.PaginationRequest.PageSize,
                cancellationToken
            );

            return result;
        }
    }
}
