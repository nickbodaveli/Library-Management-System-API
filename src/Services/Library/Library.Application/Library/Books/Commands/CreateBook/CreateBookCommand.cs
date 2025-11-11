using BuildingBlocks.CQRS;

namespace Library.Application.Library.Books.Commands.CreateBook
{
    public record CreateBookResult(Guid Id);
    public record CreateBookCommand : ICommand<CreateBookResult>
    {
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public int PublicationYear { get; init; }
        public int TotalCopies { get; init; }
    }
}
