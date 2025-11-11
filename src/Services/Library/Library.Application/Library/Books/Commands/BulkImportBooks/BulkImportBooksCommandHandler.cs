using BuildingBlocks.CQRS;
using Library.Application.Common.Models;
using Library.Application.Data;
using Library.Domain.Models.Book;
using System.Collections.Concurrent;

namespace Library.Application.Library.Books.Commands.BulkImportBooks
{
    public class BulkImportBooksHandler(IBookRepository _bookRepository) : ICommandHandler<BulkImportBooksCommand, BulkImportResult>
    {
        public async Task<BulkImportResult> Handle(BulkImportBooksCommand request, CancellationToken cancellationToken)
        {
            var booksToInsert = new ConcurrentBag<Book>();
            var failedRecords = new ConcurrentBag<string>();

            await Task.Run(() =>
            {
                Parallel.ForEach(request.Records, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, record =>
                {
                    try
                    {
                        var book = Book.Create(
                            title: record.Title,
                            author: record.Author,
                            isbn: record.ISBN,
                            publicationYear: record.PublicationYear,
                            totalCopies: record.TotalCopies
                        );

                        booksToInsert.Add(book);
                    }
                    catch (Exception ex)
                    {
                        failedRecords.Add($"ISBN: {record.ISBN}, Error: {ex.Message}");
                    }
                });
            });

            if (booksToInsert.Any())
            {
                await _bookRepository.BulkInsertAsync(booksToInsert, cancellationToken);
                await _bookRepository.SaveChangesAsync(cancellationToken);
            }

            return new BulkImportResult(
                TotalProcessed: request.Records.Count,
                TotalSuccessful: booksToInsert.Count,
                TotalFailed: failedRecords.Count,
                FailedRecords: failedRecords.ToList()
            );
        }
    }
}
