using Domain.Abstractions.Abstractions;
using Library.Domain.ValueObjects.Member;

namespace Library.Domain.Events
{
    public record MemberCreatedDomainEvent(
        MemberId MemberId,
        string FirstName,
        string LastName,
        string Email) : IDomainEvent;
}
