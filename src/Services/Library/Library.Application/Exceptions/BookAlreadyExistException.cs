using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class BookAlreadyExistException : AlreadyExistsException
    {
        public BookAlreadyExistException(string text) : base(text)
        {
            
        }
    }
}
