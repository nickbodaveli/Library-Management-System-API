using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Application.Common.Dtos
{
    [BsonIgnoreExtraElements]
    public record LoanReadDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; init; }

        [BsonElement("bookId")]
        [BsonRepresentation(BsonType.String)]
        public Guid BookId { get; init; }

        [BsonElement("memberId")]
        [BsonRepresentation(BsonType.String)]
        public Guid MemberId { get; init; }

        [BsonElement("loanDate")]
        public DateTime LoanDate { get; init; }

        [BsonElement("dueDate")]
        public DateTime DueDate { get; init; }

        [BsonElement("returnDate")]
 
        [BsonIgnoreIfNull]
        public DateTime? ReturnDate { get; init; }

        [BsonElement("status")]
        public string Status { get; init; }
    }
}
