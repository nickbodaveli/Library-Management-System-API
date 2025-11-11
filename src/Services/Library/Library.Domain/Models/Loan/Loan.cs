using Domain.Abstractions.Abstractions;
using Library.Application.EventHandlers.Domain;
using Library.Domain.Events;
using Library.Domain.Exceptions;
using Library.Domain.ValueObjects.Book;
using Library.Domain.ValueObjects.Loan;
using Library.Domain.ValueObjects.Member;

namespace Library.Domain.Models.Loan
{
    public class Loan : Aggregate<LoanId>
    {
        public BookId BookId { get; private set; }
        public MemberId MemberId { get; private set; }
        public DateTime LoanDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public string Status { get; private set; } = "Borrowed";

        private Loan() { }

        public Loan(
            LoanId id,
            BookId bookId,
            MemberId memberId,
            DateTime dueDate)
        {
            Id = id;
            BookId = bookId;
            MemberId = memberId;

            LoanDate = DateTime.UtcNow.Date;
            DueDate = dueDate.Date; 
            ReturnDate = null;
            Status = "Borrowed";

            AddDomainEvent(new LoanCreatedDomainEvent(id.Value, bookId.Value, memberId.Value, LoanDate, DueDate));
        }

        public static Loan Create(BookId bookId, MemberId memberId, DateTime dueDate)
        {
            return new Loan(LoanId.New(), bookId, memberId, dueDate);
        }

        public void MarkAsReturned()
        {
            if (ReturnDate.HasValue)
            {
                throw new DomainException("Loan has already been returned.");
            }

            DateTime actualReturnDate = DateTime.UtcNow.Date;

            if (actualReturnDate < LoanDate)
            {
                throw new DomainException("Return date cannot be before loan date.");
            }

            ReturnDate = actualReturnDate;

            if (ReturnDate > DueDate)
            {
                Status = "Returned (Overdue)";
            }
            else
            {
                Status = "Returned";
            }

            AddDomainEvent(new LoanReturnedDomainEvent(
                this.Id.Value,
                this.BookId.Value,     
                this.MemberId.Value,   
                this.ReturnDate.Value  
            ));
        }
    }
}
