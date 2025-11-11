using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Domain.Models.Member;
using Library.Domain.ValueObjects.Member;
using MediatR;

namespace Library.Application.Library.Members.Commands.CreateMember
{
    public class CreateMemberCommandHandler(IMemberRepository _memberRepository) : IRequestHandler<CreateMemberCommand, CreateMemberResult>
    {
        public async Task<CreateMemberResult> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            if (await _memberRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new EmailAlreadyExistsException($"Email '{request.Email}' is already registered.");
            }

            var memberId = MemberId.Of(Guid.NewGuid());

            var member = Member.Create(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email
            );

            await _memberRepository.AddAsync(member, cancellationToken);
            await _memberRepository.SaveChangesAsync(cancellationToken);

            return new CreateMemberResult(member.Id.Value);
        }
    }
}
