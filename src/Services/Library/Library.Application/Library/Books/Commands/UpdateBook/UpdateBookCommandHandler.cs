using BuildingBlocks.CQRS;
using Library.Application.Data;
using Library.Application.Exceptions;

namespace Library.Application.Library.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler(IBookRepository _bookRepository) : ICommandHandler<UpdateBookCommand, UpdateBookResult>
    {
        public async Task<UpdateBookResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);

            if (book is null)
            {
                throw new BookAlreadyExistException($"Book with ISBN '{request.ISBN}' already exists.");
            }

            if (book.ISBN != request.ISBN && await _bookRepository.ExistsByISBNAsync(request.ISBN, cancellationToken))
            {
                throw new BookAlreadyInUseException($"ISBN '{request.ISBN}' is already in use by another book.");
            }

            book.UpdateDetails(
                request.Title,
                request.Author,
                request.ISBN,
                request.TotalCopies
            );

            await _bookRepository.UpdateAsync(book, cancellationToken);
            await _bookRepository.SaveChangesAsync(cancellationToken);

            return new UpdateBookResult(true);
        }
    }
}
