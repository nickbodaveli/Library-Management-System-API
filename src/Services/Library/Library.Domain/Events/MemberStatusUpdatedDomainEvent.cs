using Domain.Abstractions.Abstractions;

namespace Library.Domain.Events
{
    public record MemberStatusUpdatedDomainEvent(
    Guid MemberId,
    bool IsActive) : IDomainEvent;
}
