using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Loans.Queries.GeMemberActiveLoans
{
    public record GetMemberActiveLoansQuery(Guid MemberId) : IQuery<List<MemberActiveLoanDto>>;
}
