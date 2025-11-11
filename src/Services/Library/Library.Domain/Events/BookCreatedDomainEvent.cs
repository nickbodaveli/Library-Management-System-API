using Domain.Abstractions.Abstractions;
using Library.Domain.ValueObjects.Book;

namespace Library.Application.EventHandlers.Domain
{
    public record BookCreatedDomainEvent(
        BookId Id,
        string Title,
        string Author,
        string ISBN,
        int PublicationYear,
        int TotalCopies) : IDomainEvent;
}
