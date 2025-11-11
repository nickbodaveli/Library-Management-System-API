using Domain.Abstractions.Abstractions;

namespace Library.Application.EventHandlers.Domain
{
    public record LoanCreatedDomainEvent(
        Guid LoanId,
        Guid BookId,
        Guid MemberId,
        DateTime LoanDate,
        DateTime DueDate) : IDomainEvent;
}
