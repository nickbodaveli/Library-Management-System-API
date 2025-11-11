using Library.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Library.Domain.ValueObjects.Book
{
    public record BookId
    {
        public Guid Value { get; }

        [JsonConstructor]
        public BookId(Guid value)
        {
            if (value == Guid.Empty)
                throw new DomainException("BookId cannot be empty.");

            Value = value;
        }

        public static BookId New() => new(Guid.NewGuid());
        public static BookId Of(Guid value) => new(value);
        public static BookId FromExisting(Guid value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
