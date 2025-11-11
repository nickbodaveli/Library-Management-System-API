using BuildingBlocks.CQRS;

namespace Library.Application.Library.Members.Commands.CreateMember
{
    public record CreateMemberResult(Guid Id);
    public record CreateMemberCommand : ICommand<CreateMemberResult>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public bool IsActive { get; init; } = false;
    }
}
