using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Application.Common.Dtos
{
    [BsonIgnoreExtraElements]
    public record MemberReadDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; init; }

        [BsonElement("firstName")]
        public string FirstName { get; init; }

        [BsonElement("lastName")]
        public string LastName { get; init; }

        [BsonElement("email")]
        public string Email { get; init; }

        [BsonElement("isActive")]
        public bool IsActive { get; init; }
    }
}
