using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class BookAlreadyInUseException : AlreadyExistsException
    {
        public BookAlreadyInUseException(string text) : base(text)
        {

        }
    }
}
