using Library.Application.Data;
using Library.Application.Exceptions;
using MediatR;

namespace Library.Application.Library.Loans.Commands.ReturnBook
{
    public class ReturnBookCommandHandler(ILoanRepository _loanRepository, IBookRepository _bookRepository): IRequestHandler<ReturnBookCommand, ReturnBookResult>
    {
        public async Task<ReturnBookResult> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken); 

            if (loan is null)
            {
                throw new LoanNotFoundException($"Loan with ID {request.LoanId} not found.");
            }

            loan.MarkAsReturned();

            var book = await _bookRepository.GetByIdAsync(loan.BookId.Value, cancellationToken);

            if (book is null)
            {
                throw new BookIsNullException("Book Is Null");
            }

            book.Return(); 

            await _loanRepository.UpdateAsync(loan, cancellationToken); 
            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _loanRepository.SaveChangesAsync(cancellationToken); 

            return new ReturnBookResult(book.Id.Value);
        }
    }
}
