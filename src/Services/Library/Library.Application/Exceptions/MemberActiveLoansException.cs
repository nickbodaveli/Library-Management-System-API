using BuildingBlocks.Exceptions;

namespace Library.Application.Exceptions
{
    public class MemberActiveLoansException : NotFoundException
    {
        public MemberActiveLoansException(string text) : base(text)
        {

        }
    }
}
