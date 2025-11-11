namespace Library.Application.Common.Models
{
    // Record class for immutable, easy-to-use data transfer
    public record BulkImportResult(
        int TotalProcessed,
        int TotalSuccessful,
        int TotalFailed,
        List<string> FailedRecords
    );
}
