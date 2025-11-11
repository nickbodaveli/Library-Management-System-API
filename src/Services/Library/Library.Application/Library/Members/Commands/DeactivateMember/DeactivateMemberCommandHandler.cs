using Library.Application.Data;
using Library.Application.Exceptions;
using MediatR;

namespace Library.Application.Library.Members.Commands.DeactivateMember
{
    public class DeactivateMemberHandler(IMemberRepository _memberRepository, ILoanRepository _loanRepository) : IRequestHandler<DeactivateMemberCommand, DeactivateMemberResult>
    {
        public async Task<DeactivateMemberResult> Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken);

            if (member is null)
            {
                throw new MemberNotFoundException($"Member with ID {request.Id} not found.");
            }

            var activeLoans = await _loanRepository.GetActiveLoanCountByMemberAsync(request.Id, cancellationToken);
            if (activeLoans > 0)
            {
                throw new MemberActiveLoansException($"Cannot deactivate member. Member has {activeLoans} outstanding loans.");
            }

            member.Deactivate();

            await _memberRepository.UpdateAsync(member, cancellationToken);
            await _memberRepository.SaveChangesAsync(cancellationToken);

            return new DeactivateMemberResult(true);
        }
    }
}
