using Library.Domain.Models.Member;
using Library.Domain.ValueObjects.Member;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Data
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IApplicationDbContext _context; 

        public MemberRepository(IApplicationDbContext context) => _context = context;

        public Task AddAsync(Member member, CancellationToken cancellationToken)
        {
            _context.Members.Add(member);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Member member, CancellationToken cancellationToken = default)
        {
            _context.Members.Update(member); 
            return Task.CompletedTask;
        }

        public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == MemberId.Of(id), cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
            await _context.Members.AnyAsync(m => m.Email == email, cancellationToken);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
