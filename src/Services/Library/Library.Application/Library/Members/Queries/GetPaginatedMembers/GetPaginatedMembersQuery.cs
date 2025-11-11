using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;

namespace Library.Application.Library.Members.Queries.GetPaginatedMembers
{
    public record GetPaginatedMembersQuery(PaginationRequest PaginationRequest) : IQuery<GetPaginatedMemberQueryResult>;

    public record GetPaginatedMemberQueryResult(PaginatedResult<MemberReadDto> Members);
}
