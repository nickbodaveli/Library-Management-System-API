using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class MemberInActiveException : NotActiveException
    {
        public MemberInActiveException(string text) : base(text)
        {

        }
    }
}
