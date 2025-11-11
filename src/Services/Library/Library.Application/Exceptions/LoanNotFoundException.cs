using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class LoanNotFoundException : NotFoundException
    {
        public LoanNotFoundException(string text) : base(text)
        {

        }
    }
}
