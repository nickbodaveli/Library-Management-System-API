using Library.Domain.Models.Member;
using Library.Domain.ValueObjects.Member;

namespace Library.Application.Data
{
    public interface IMemberRepository : IBaseRepository<Member, MemberId>
    {
        Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(Member member, CancellationToken cancellationToken = default);
    }
}
