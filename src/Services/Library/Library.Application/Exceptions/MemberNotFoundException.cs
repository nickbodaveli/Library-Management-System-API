using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class MemberNotFoundException : NotFoundException
    {
        public MemberNotFoundException(string text) : base(text)
        {

        }
    }
}
