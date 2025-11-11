using BuildingBlocks.CQRS;
using Library.Application.Common.Models;

namespace Library.Application.Library.Books.Commands.BulkImportBooks
{
    public record BulkBookRecord
    {
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public int PublicationYear { get; init; }
        public int TotalCopies { get; init; }
    }

    public record BulkImportBooksCommand : ICommand<BulkImportResult>
    {
        public List<BulkBookRecord> Records { get; init; } = new();
    }
}
