namespace Library.Application.Common.Dtos
{
    public record MemberActiveLoanDto(
        Guid LoanId,
        string BookTitle,
        string BookAuthor,
        DateTime LoanDate,
        DateTime DueDate
    );
}
