using Library.Application.Data;
using MediatR;

namespace Library.Application.Library.Members.Queries.GetPaginatedMembers
{
    public class GetPaginatedMembersQueryHandler(IMemberReadRepository _memberReadRepository) : IRequestHandler<GetPaginatedMembersQuery, GetPaginatedMemberQueryResult>
    {
        public Task<GetPaginatedMemberQueryResult> Handle(
            GetPaginatedMembersQuery request,
            CancellationToken cancellationToken)
        {
            return _memberReadRepository.GetPaginatedMembersAsync(
                request.PaginationRequest.PageIndex,
                request.PaginationRequest.PageSize,
                cancellationToken
            );
        }
    }
}
