using BuildingBlocks.Pagination;
using Library.Application.Library.Loans.Commands.BorrowBook;
using Library.Application.Library.Loans.Commands.ReturnBook;
using Library.Application.Library.Loans.Queries.GeMemberActiveLoans;
using Library.Application.Library.Loans.Queries.GetOverDueLoans;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    /// <summary>
    /// API Controller for managing all loan-related operations, including borrowing, returning,
    /// and querying active and overdue loans.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoansController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR instance for sending commands and queries.</param>
        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record BorrowBookResponse(Guid LoanId);

        /// <summary>
        /// Creates a new loan record, allowing a member to borrow a book.
        /// </summary>
        /// <param name="command">The details required to borrow a book (e.g., Member ID, Book ID).</param>
        /// <returns>A 201 Created status with the ID of the newly created loan.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command)
        {
            var result = await _mediator.Send(command);

            var response = result.Adapt<BorrowBookResponse>();

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing loan record, marking the book as returned.
        /// </summary>
        /// <param name="command">The details required to return a book (e.g., Loan ID).</param>
        /// <returns>A 200 OK status upon successful return.</returns>
        [HttpPut("return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of all currently active loans for a specific member.
        /// </summary>
        /// <param name="request">The query containing the unique identifier of the member.</param>
        /// <returns>A 200 OK status with the list of active loans, or 404 Not Found if the member is invalid.</returns>
        [HttpGet("{memberId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberActiveLoans([FromRoute] GetMemberActiveLoansQuery request)
        {
            var activeLoans = await _mediator.Send(request);

            if (activeLoans is null)
                return NotFound();

            return Ok(activeLoans);
        }

        /// <summary>
        /// Retrieves a paginated list of all overdue loans in the library system.
        /// </summary>
        /// <param name="request">The pagination parameters (e.g., page number, page size).</param>
        /// <returns>A 200 OK status with the paginated list of overdue loans.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOverDueLoans([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetPaginatedOverDueLoansQuery(request));

            var response = result.Adapt<GetPaginatedOverDueLoansQueryResult>();

            return Ok(response);
        }
    }
}