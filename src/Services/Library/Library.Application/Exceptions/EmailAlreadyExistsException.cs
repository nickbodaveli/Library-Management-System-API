using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class EmailAlreadyExistsException : AlreadyExistsException
    {
        public EmailAlreadyExistsException(string text) : base(text)
        {

        }
    }
}
