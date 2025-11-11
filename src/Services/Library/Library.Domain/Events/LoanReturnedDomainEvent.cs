using Domain.Abstractions.Abstractions;

namespace Library.Domain.Events
{
    public record LoanReturnedDomainEvent(
          Guid LoanId,
          Guid BookId,
          Guid MemberId,
          DateTime ReturnDate) : IDomainEvent;
}
