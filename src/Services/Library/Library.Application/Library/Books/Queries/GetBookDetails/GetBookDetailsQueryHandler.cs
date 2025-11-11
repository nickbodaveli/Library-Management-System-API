using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;
using Library.Application.Data;

namespace Library.Application.Library.Books.Queries.GetBookDetails
{
    public class GetBookDetailsQueryHandler(IBookReadRepository _bookReadRepository) : IQueryHandler<GetBookDetailsQuery, BookReadDto?>
    {
        public async Task<BookReadDto?> Handle(GetBookDetailsQuery request,CancellationToken cancellationToken)
        {
            return await _bookReadRepository.GetBookDtoByIdAsync(
                request.Id,
                cancellationToken
            );
        }
    }
}
