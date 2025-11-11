using Library.Application.Common.Dtos;
using Library.Application.Library.Members.Queries.GetPaginatedMembers;

namespace Library.Application.Data
{
    public interface IMemberReadRepository
    {
        Task<MemberReadDto?> GetMemberDtoByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<GetPaginatedMemberQueryResult> GetPaginatedMembersAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task AddAsync(MemberReadDto readModel, CancellationToken cancellationToken);

        Task UpdateAsync(MemberReadDto readModel, CancellationToken cancellationToken);

        Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken);
    }
}
