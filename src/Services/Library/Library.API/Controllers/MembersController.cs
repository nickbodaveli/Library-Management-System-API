using BuildingBlocks.Pagination;
using Library.Application.Library.Members.Commands.CreateMember;
using Library.Application.Library.Members.Commands.DeactivateMember;
using Library.Application.Library.Members.Commands.UpdateMemberInfo;
using Library.Application.Library.Members.Queries.GetMemberDetails;
using Library.Application.Library.Members.Queries.GetPaginatedMembers;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    /// <summary>
    /// API Controller for managing library member accounts, including creation, retrieval, 
    /// updating, and deactivation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ISender _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR sender instance for dispatching commands and queries.</param>
        public MembersController(ISender mediator)
        {
            _mediator = mediator;
        }

        public record MemberResponse(Guid Id);

        /// <summary>
        /// Creates a new library member account.
        /// </summary>
        /// <param name="command">The command containing the required member details.</param>
        /// <returns>A 201 Created status with the ID of the newly created member.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberCommand command)
        {
            var result = await _mediator.Send(command);
            var response = result.Adapt<MemberResponse>();
            // NOTE: For full REST compliance, this should return StatusCode(201, ...)
            return Ok(response);
        }

        /// <summary>
        /// Retrieves detailed information for a specific member using their unique identifier.
        /// </summary>
        /// <param name="request">The query containing the Member ID.</param>
        /// <returns>A 200 OK status with the member details, or 404 Not Found if the member does not exist.</returns>
        [HttpGet("{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberDetails([FromRoute] GetMemberDetailsQuery request)
        {
            var memberDto = await _mediator.Send(request);

            if (memberDto is null)
                return NotFound();

            return Ok(memberDto);
        }

        /// <summary>
        /// Retrieves a paginated list of all library members.
        /// </summary>
        /// <param name="request">The pagination parameters (e.g., page number, page size).</param>
        /// <returns>A 200 OK status with the paginated list of members.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaginatedMembers([FromQuery] PaginationRequest request)
        {
            var result = await _mediator.Send(new GetPaginatedMembersQuery(request));
            var response = result.Adapt<GetPaginatedMemberQueryResult>();
            return Ok(response);
        }

        public record MemberUpdateResponse(bool IsSuccess);

        /// <summary>
        /// Updates the core information (e.g., name, address) for an existing member account.
        /// </summary>
        /// <param name="command">The command containing the updated member details.</param>
        /// <returns>A 200 OK status with a success indicator, or 404 Not Found if the member does not exist.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMemberInfo([FromBody] UpdateMemberInfoCommand command)
        {
            var result = await _mediator.Send(command);
            var response = result.Adapt<MemberUpdateResponse>();
            return Ok(response);
        }

        public record DeactivateMemberResponse(bool IsSuccess);

        /// <summary>
        /// Deactivates a member's account, preventing them from borrowing books.
        /// </summary>
        /// <param name="command">The command containing the Member ID to deactivate.</param>
        /// <returns>A 200 OK status with a success indicator, or 404 Not Found if the member does not exist.</returns>
        [HttpPut("deactivate-member")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateMemberInfo([FromBody] DeactivateMemberCommand command)
        {
            var result = await _mediator.Send(command);
            var response = result.Adapt<DeactivateMemberResponse>();
            return Ok(response);
        }
    }
}