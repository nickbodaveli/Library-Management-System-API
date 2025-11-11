using BuildingBlocks.CQRS;

namespace Library.Application.Library.Loans.Commands.BorrowBook
{
    public record BorrowBookResult(Guid LoanId);
    public record BorrowBookCommand : ICommand<BorrowBookResult>
    {
        public Guid BookId { get; init; } = default!;
        public Guid MemberId { get; init; } = default!;
        public int LoanPeriodDays { get; init; } = 14;
    }
}
