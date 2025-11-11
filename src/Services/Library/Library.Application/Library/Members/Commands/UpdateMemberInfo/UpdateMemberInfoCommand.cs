using BuildingBlocks.CQRS;

namespace Library.Application.Library.Members.Commands.UpdateMemberInfo
{
    public record UpdateMemberResult(bool IsSuccess);
    public record UpdateMemberInfoCommand : ICommand<UpdateMemberResult>
    {
        public Guid Id { get; init; } = default!;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
}
