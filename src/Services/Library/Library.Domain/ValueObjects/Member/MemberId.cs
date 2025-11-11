using Library.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Library.Domain.ValueObjects.Member
{
    public record MemberId
    {
        public Guid Value { get; }

        [JsonConstructor]
        public MemberId(Guid value)
        {
            if (value == Guid.Empty)
                throw new DomainException("MemberId cannot be empty.");

            Value = value;
        }

        public static MemberId New() => new(Guid.NewGuid());
        public static MemberId Of(Guid value) => new(value);
        public static MemberId FromExisting(Guid value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
