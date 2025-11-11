using BuildingBlocks.Pagination;
using Library.Application.Library.Books.Commands.BulkImportBooks;
using Library.Application.Library.Books.Commands.CreateBook;
using Library.Application.Library.Books.Commands.UpdateBook;

using Library.Application.Library.Books.Queries.GetBookDetails;
using Library.Application.Library.Books.Queries.GetPaginatedBooks;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    /// <summary>
    /// API Controller for managing all book-related operations, including creation, updates,
    /// bulk importing, and retrieving details and paginated lists.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooksController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR sender instance for dispatching commands and queries.</param>
        public BooksController(ISender mediator)
        {
            _mediator = mediator;
        }

        public record BookResponse(Guid Id);

        /// <summary>
        /// Creates a new book record in the system.
        /// </summary>
        /// <param name="command">The command containing the details for the new book.</param>
        /// <returns>A 201 Created status with the ID of the newly created book.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            var result = await _mediator.Send(command);

            var response = result.Adapt<BookResponse>();
            // NOTE: Must be corrected to return 201 Created and location header for full REST compliance.
            return Ok(response);
        }

        /// <summary>
        /// Retrieves detailed information for a specific book using its unique identifier.
        /// </summary>
        /// <param name="request">The query containing the Book ID.</param>
        /// <returns>A 200 OK status with the book details, or 404 Not Found if the book does not exist.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookDetails([FromRoute] GetBookDetailsQuery request)
        {
            var bookDto = await _mediator.Send(request);

            if (bookDto is null)
            {
                return NotFound();
            }

            return Ok(bookDto);
        }

        /// <summary>
        /// Retrieves a paginated list of all books in the library catalogue.
        /// </summary>
        /// <param name="request">The pagination parameters (e.g., page number, page size).</param>
        /// <returns>A 200 OK status with the paginated list of books.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaginatedBooks([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetPaginatedBooksQuery(request));

            var response = result.Adapt<GetPaginatedBooksQueryResult>();

            return Ok(response);
        }

        public record BookUpdateResponse(bool IsSuccess);

        /// <summary>
        /// Updates the information for an existing book record.
        /// </summary>
        /// <param name="command">The command containing the updated book details.</param>
        /// <returns>A 200 OK status with a success indicator, or 404 Not Found if the book does not exist.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBookInfo([FromBody] UpdateBookCommand command)
        {
            var result = await _mediator.Send(command);

            var response = result.Adapt<BookUpdateResponse>();

            return Ok(response);
        }

        public record BulkImportResponse(
          int TotalProcessed,
          int TotalSuccessful,
          int TotalFailed,
          List<string> FailedRecords
        );

        /// <summary>
        /// Processes a bulk upload to import multiple new book records.
        /// </summary>
        /// <param name="command">The command containing the data for bulk import.</param>
        /// <returns>A 200 OK status with a summary of the import results (processed, successful, and failed records).</returns>
        [HttpPost("bulk-import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkImportBooks([FromBody] BulkImportBooksCommand command)
        {
            var result = await _mediator.Send(command);

            var response = result.Adapt<BulkImportResponse>();

            return Ok(response);
        }
    }
}