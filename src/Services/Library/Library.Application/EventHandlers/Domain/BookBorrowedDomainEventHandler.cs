using Library.Application.Common.Dtos;
using Library.Application.Data;
using Library.Domain.Events;
using Library.Domain.ValueObjects.Loan;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class BookBorrowedDomainEventHandler : INotificationHandler<BookBorrowedDomainEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookReadRepository _bookReadRepository; 
        private readonly ILoanReadRepository _loanReadRepository;

        public BookBorrowedDomainEventHandler(
            IBookRepository bookRepository,
            ILoanRepository loanRepository,
            IBookReadRepository bookReadRepository,
            ILoanReadRepository loanReadRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _bookReadRepository = bookReadRepository;
            _loanReadRepository = loanReadRepository;
        }

        public async Task Handle(BookBorrowedDomainEvent notification, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(notification.BookId, cancellationToken);
            if (book is null) return;

            book.DecreaseAvailableCount(); 

            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _bookRepository.SaveChangesAsync(cancellationToken);

            await _bookReadRepository.UpdateAvailableCountAsync(
                notification.BookId,
                -1, 
                cancellationToken);

            var loan = await _loanRepository.GetByIdAsync(notification.LoanId, cancellationToken);

            if (loan is null) return;

            var loanReadDto = new LoanReadDto {
                Id = loan.Id.Value,
                BookId = loan.BookId.Value,
                MemberId = loan.MemberId.Value,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = "Active"
            };

            await _loanReadRepository.AddAsync(loanReadDto, cancellationToken);
        }
    }
}
