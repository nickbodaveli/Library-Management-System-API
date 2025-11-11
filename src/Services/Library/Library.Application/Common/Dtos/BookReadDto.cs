using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Application.Common.Dtos
{
    [BsonIgnoreExtraElements]
    public record BookReadDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; init; }

        [BsonElement("title")]
        public string Title { get; init; }

        [BsonElement("author")]
        public string Author { get; init; }

        [BsonElement("isbn")]
        public string ISBN { get; init; }

        [BsonElement("publicationYear")]
        public int PublicationYear { get; init; }

        [BsonElement("totalCopies")]
        public int TotalCopies { get; init; }

        [BsonElement("availableCopies")]
        public int AvailableCopies { get; init; }

        [BsonElement("isArchived")]
        public bool IsArchived { get; init; }
    }
}
