using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class MemberMaximumLimitException : MaximumLimitException
    {
        public MemberMaximumLimitException(string text) : base(text)
        {

        }
    }
}
