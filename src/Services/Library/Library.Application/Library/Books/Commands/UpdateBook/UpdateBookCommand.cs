using BuildingBlocks.CQRS;

namespace Library.Application.Library.Books.Commands.UpdateBook
{
    public record UpdateBookResult(bool IsSuccess);
    public record UpdateBookCommand : ICommand<UpdateBookResult>
    {
        public Guid Id { get; init; } = default!;
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public int PublicationYear { get; init; }
        public int TotalCopies { get; init; }
    }
}
