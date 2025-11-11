using Library.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Library.Domain.ValueObjects.Loan
{
    public record LoanId
    {
        public Guid Value { get; }

        [JsonConstructor]
        public LoanId(Guid value)
        {
            if (value == Guid.Empty)
                throw new DomainException("LoanId cannot be empty.");

            Value = value;
        }

        public static LoanId New() => new(Guid.NewGuid());
        public static LoanId Of(Guid value) => new(value);
        public static LoanId FromExisting(Guid value) => new(value);

        public override string ToString() => Value.ToString();
    }
}
