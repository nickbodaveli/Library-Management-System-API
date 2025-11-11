using BuildingBlocks.CQRS;

namespace Library.Application.Library.Members.Commands.DeactivateMember
{
    public record DeactivateMemberResult(bool IsSuccess);
    public record DeactivateMemberCommand : ICommand<DeactivateMemberResult>
    {
        public Guid Id { get; init; } = default!;
    }
}
