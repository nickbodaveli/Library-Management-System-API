using Library.Application.Common.Dtos;
using Library.Application.Data;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class BookCreatedDomainEventHandler(IBookReadRepository bookReadRepository)
        : INotificationHandler<BookCreatedDomainEvent>
    {
        public async Task Handle(BookCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var readModel = new BookReadDto {
                Id = notification.Id.Value,
                Title = notification.Title,
                Author = notification.Author,
                ISBN = notification.ISBN,
                PublicationYear = notification.PublicationYear,
                AvailableCopies = notification.TotalCopies,
                TotalCopies = notification.TotalCopies,
                IsArchived = false
            };
          
            await bookReadRepository.AddAsync(readModel, cancellationToken);
        }
    }
}
