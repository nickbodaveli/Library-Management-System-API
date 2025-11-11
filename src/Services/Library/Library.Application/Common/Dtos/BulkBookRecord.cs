namespace Library.Application.Common.Dtos
{
    public record BulkBookRecord
    {
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public string ISBN { get; init; } = string.Empty;
        public int PublicationYear { get; init; }
        public int TotalCopies { get; init; }
    }
}
