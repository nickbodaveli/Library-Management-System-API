using BuildingBlocks.CQRS;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Domain.Models.Book;

namespace Library.Application.Library.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler(IBookRepository _bookRepository) : ICommandHandler<CreateBookCommand, CreateBookResult>
    {
        public async Task<CreateBookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if (await _bookRepository.ExistsByISBNAsync(request.ISBN, cancellationToken))
            {
                throw new BookAlreadyExistException($"Book with ISBN '{request.ISBN}' already exists.");
            }

            var book = Book.Create(
                title: request.Title,
                author: request.Author,
                isbn: request.ISBN,
                publicationYear: request.PublicationYear,
                totalCopies: request.TotalCopies
            );

            await _bookRepository.AddAsync(book, cancellationToken);
            await _bookRepository.SaveChangesAsync(cancellationToken);

            return new CreateBookResult(book.Id.Value);
        }
    }
}
