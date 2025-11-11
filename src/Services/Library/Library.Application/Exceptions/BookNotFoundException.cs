using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(string text) : base(text)
        {

        }
    }
}
