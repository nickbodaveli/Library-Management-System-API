using Domain.Abstractions.Abstractions;

namespace Library.Domain.Events
{
    public record MemberUpdatedDomainEvent(
     Guid MemberId,
     string FirstName,
     string LastName,
     string Email,
     bool IsActive) : IDomainEvent;
}
