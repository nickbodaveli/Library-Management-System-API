using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Books.Queries.GetBookDetails
{
    public record GetBookDetailsQuery(Guid Id) : IQuery<BookReadDto?>;
}
