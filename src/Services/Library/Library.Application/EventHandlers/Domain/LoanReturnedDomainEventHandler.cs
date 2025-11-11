using Library.Application.Data;
using Library.Domain.Events;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class LoanReturnedDomainEventHandler : INotificationHandler<LoanReturnedDomainEvent>
    {
        private readonly ILoanReadRepository _loanReadRepository;
        private readonly IBookReadRepository _bookReadRepository; 

        public LoanReturnedDomainEventHandler(
            ILoanReadRepository loanReadRepository,
            IBookReadRepository bookReadRepository)
        {
            _loanReadRepository = loanReadRepository;
            _bookReadRepository = bookReadRepository;
        }

        public async Task Handle(LoanReturnedDomainEvent notification, CancellationToken cancellationToken)
        {
            var loan = await _loanReadRepository.GetLoanDtoByIdAsync(notification.LoanId, cancellationToken);

            if (loan == null)
            {
                return;
            }

            var updatedLoanDto = loan with
            {
                ReturnDate = notification.ReturnDate, 
                Status = "Returned"                  
            };

            await _loanReadRepository.UpdateAsync(updatedLoanDto, cancellationToken);

            await _bookReadRepository.UpdateAvailableCountAsync(
                notification.BookId,
                1, 
                cancellationToken);
        }
    }
}
