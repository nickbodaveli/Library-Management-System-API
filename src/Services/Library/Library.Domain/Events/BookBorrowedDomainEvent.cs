using Domain.Abstractions.Abstractions;

namespace Library.Domain.Events
{
    public record BookBorrowedDomainEvent(
         Guid BookId,
         Guid LoanId,
         Guid MemberId) : IDomainEvent;
}
