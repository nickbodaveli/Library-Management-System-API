using Library.Application.Data;
using Library.Domain.Events;
using MediatR;

namespace Library.Application.EventHandlers.Domain
{
    public class MemberStatusUpdatedDomainEventHandler(IMemberReadRepository memberReadRepository) : INotificationHandler<MemberStatusUpdatedDomainEvent>
    {
        public async Task Handle(MemberStatusUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var member = await memberReadRepository.GetMemberDtoByIdAsync(notification.MemberId, cancellationToken);

            if (member == null)
            {
                return;
            }

            var memberReadDto = member with
            {
                IsActive = notification.IsActive
            };

            await memberReadRepository.UpdateAsync(memberReadDto, cancellationToken);
        }
    }
}
