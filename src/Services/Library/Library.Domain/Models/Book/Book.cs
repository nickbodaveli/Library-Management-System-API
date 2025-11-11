using Domain.Abstractions.Abstractions;
using Library.Application.EventHandlers.Domain;
using Library.Domain.Events;
using Library.Domain.Exceptions;
using Library.Domain.ValueObjects.Book;
using Library.Domain.ValueObjects.Loan;
using Library.Domain.ValueObjects.Member;

namespace Library.Domain.Models.Book
{
    public class Book : Aggregate<BookId>
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string ISBN { get; private set; }
        public int PublicationYear { get; private set; }
        public int TotalCopies { get; private set; }
        public int AvailableCopies { get; private set; } 
        public bool IsArchived { get; private set; }

        public int Version { get; private set; } = 0;

        private Book() { }

        public Book(
            BookId id,
            string title,
            string author,
            string isbn,
            int publicationYear,
            int totalCopies)
        {
            Id = id;
            Title = title;
            Author = author;
            ISBN = isbn;
            PublicationYear = publicationYear;
            TotalCopies = totalCopies;
            AvailableCopies = totalCopies;
            IsArchived = false;
        }

        public static Book Create(
            string title,
            string author,
            string isbn,
            int publicationYear,
            int totalCopies)
        {
            var book = new Book(
                BookId.New(),
                title,
                author,
                isbn,
                publicationYear,
                totalCopies
            );

            book.AddDomainEvent(new BookCreatedDomainEvent(
                book.Id, book.Title, book.Author, book.ISBN, book.PublicationYear, book.TotalCopies));

            return book;
        }
        public void UpdateDetails(
            string title,
            string author,
            string isbn,
            int totalCopies)
        {
            if (totalCopies < 0) throw new InvalidOperationException("Total copies cannot be negative.");

            int borrowedCopies = this.TotalCopies - this.AvailableCopies;

            if (totalCopies < borrowedCopies)
            {
                throw new DomainException($"Cannot reduce total copies below {borrowedCopies} currently borrowed copies.");
            }

            this.AvailableCopies = totalCopies - borrowedCopies;

            this.Title = title;
            this.Author = author;
            this.ISBN = isbn;
            this.TotalCopies = totalCopies;

            AddDomainEvent(new BookUpdatedDomainEvent(
                this.Id,
                this.Title,
                this.Author,
                this.ISBN,
                this.PublicationYear,
                this.TotalCopies,
                this.AvailableCopies));
        }

        public void Borrow(LoanId loanId, MemberId memberId)
        {
            if (AvailableCopies <= 0)
            {
                throw new DomainException("Book has no available copies to borrow.");
            }

            AvailableCopies--;

            AddDomainEvent(new BookBorrowedDomainEvent(
                this.Id.Value,
                loanId.Value,
                memberId.Value));
        }

        public void Return()
        {
            if (AvailableCopies < TotalCopies)
            {
                AvailableCopies++;
            }
        }

        public bool CanBeBorrowed() => AvailableCopies > 0 && !IsArchived;

        public void DecreaseAvailableCount()
        {
            if (AvailableCopies <= 0)
            {
                throw new InvalidOperationException("Available count cannot be negative.");
            }
            AvailableCopies--;
        }
    }
}
