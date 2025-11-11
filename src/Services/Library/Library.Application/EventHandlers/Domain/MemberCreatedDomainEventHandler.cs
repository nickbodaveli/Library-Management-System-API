using Library.Application.Common.Dtos;
using Library.Application.Data;
using MediatR;

namespace Library.Domain.Events
{
    public class MemberCreatedDomainEventHandler(IMemberReadRepository memberReadRepository) : INotificationHandler<MemberCreatedDomainEvent>
    {
        public async Task Handle(MemberCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var member = new MemberReadDto {
                Id = notification.MemberId.Value,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                Email = notification.Email,
                IsActive = true
            };

            await memberReadRepository.AddAsync(member, cancellationToken);
        }
    }
}
