using BuildingBlocks.CQRS;
using Library.Application.Common.Dtos;
using Library.Application.Data;

namespace Library.Application.Library.Members.Queries.GetMemberDetails
{
    public class GetMemberDetailsQueryHandler : IQueryHandler<GetMemberDetailsQuery, MemberReadDto?>
    {
        private readonly IMemberReadRepository _memberReadRepository;

        public GetMemberDetailsQueryHandler(IMemberReadRepository memberReadRepository)
        {
            _memberReadRepository = memberReadRepository;
        }

        public Task<MemberReadDto?> Handle(GetMemberDetailsQuery request, CancellationToken cancellationToken)
        {
            return _memberReadRepository.GetMemberDtoByIdAsync(
                request.Id,
                cancellationToken
            );
        }
    }
}
