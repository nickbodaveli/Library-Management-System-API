using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class BookIsNullException : NullException
    {
        public BookIsNullException(string text) : base(text)
        {

        }
    }
}
