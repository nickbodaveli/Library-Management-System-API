using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Members.Queries.GetMemberDetails
{
    public record GetMemberDetailsQuery(Guid Id) : IQuery<MemberReadDto?>;
}
