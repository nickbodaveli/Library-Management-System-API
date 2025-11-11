using Library.Application.Data;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class BookUpdatedDomainEventHandler(IBookReadRepository bookReadRepository) : INotificationHandler<BookUpdatedDomainEvent>
    {
        public async Task Handle(BookUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var book = await bookReadRepository.GetBookDtoByIdAsync(notification.Id.Value, cancellationToken);

            if (book == null)
            {
                return;
            }

            var updatedReadModel = book with
            {
                Title = notification.Title,
                Author = notification.Author,
                ISBN = notification.ISBN,
                PublicationYear = notification.PublicationYear,
                TotalCopies = notification.TotalCopies,
            };

            await bookReadRepository.UpdateAsync(updatedReadModel, cancellationToken);
        }
    }
}
