using BuildingBlocks.CQRS;

namespace Library.Application.Library.Loans.Commands.ReturnBook
{
    public record ReturnBookResult(Guid Id);
    public record ReturnBookCommand : ICommand<ReturnBookResult>
    {
        public Guid LoanId { get; init; } = default!;
        public DateTime ReturnDate { get; init; } = DateTime.UtcNow;
    }
}
