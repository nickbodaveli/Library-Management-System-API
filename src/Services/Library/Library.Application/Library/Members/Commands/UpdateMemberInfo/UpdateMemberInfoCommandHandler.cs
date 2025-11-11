using BuildingBlocks.CQRS;
using Library.Application.Data;
using Library.Application.Exceptions;

namespace Library.Application.Library.Members.Commands.UpdateMemberInfo
{
    public class UpdateMemberInfoCommandHandler(IMemberRepository _memberRepository) : ICommandHandler<UpdateMemberInfoCommand, UpdateMemberResult>
    {
        public async Task<UpdateMemberResult> Handle(UpdateMemberInfoCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken);

            if (member is null)
            {
                throw new MemberNotFoundException($"Member with ID {request.Id} not found for update.");
            }

            if (member.Email != request.Email)
            {
                var exists = await _memberRepository.ExistsByEmailAsync(request.Email, cancellationToken);
                if (exists)
                {
                    throw new EmailAlreadyExistsException($"Email '{request.Email}' is already used by another member.");
                }
            }

            member.UpdateInfo(
                request.FirstName,
                request.LastName,
                request.Email,
                request.IsActive
            );

            await _memberRepository.UpdateAsync(member, cancellationToken);
            await _memberRepository.SaveChangesAsync(cancellationToken);

            return new UpdateMemberResult(true);
        }
    }
}
