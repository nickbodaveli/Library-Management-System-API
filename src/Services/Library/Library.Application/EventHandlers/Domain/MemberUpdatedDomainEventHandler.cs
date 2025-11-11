using Library.Application.Data;
using Library.Domain.Events;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class MemberUpdatedDomainEventHandler(IMemberReadRepository memberReadRepository) : INotificationHandler<MemberUpdatedDomainEvent>
    {
        public async Task Handle(MemberUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var existingMember = await memberReadRepository.GetMemberDtoByIdAsync(notification.MemberId, cancellationToken);

            if (existingMember == null)
            {
                return;
            }

            var memberReadDto = existingMember with
            {
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Email = notification.Email,
            };

            await memberReadRepository.UpdateAsync(memberReadDto, cancellationToken);
        }
    }
}
