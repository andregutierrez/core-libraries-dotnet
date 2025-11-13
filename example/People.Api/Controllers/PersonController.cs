using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.UseCases.Persons.Commands;
using People.Application.UseCases.Persons.Queries.GetPersonByKey;
using People.Application.UseCases.Persons.Queries.SearchPersons;

namespace People.Api.Controllers;

/// <summary>
/// Controller for managing persons.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of <see cref="PersonController"/>.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public PersonController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets a person by its alternate key.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed person information, or 404 if not found.</returns>
    [HttpGet("{personKey:guid}")]
    [ProducesResponseType(typeof(GetPersonByKeyQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPersonByKeyQueryResponse>> GetByKey(
        Guid personKey,
        CancellationToken cancellationToken)
    {
        var query = new GetPersonByKeyQuery { PersonKey = personKey };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Searches persons with filters and pagination.
    /// </summary>
    /// <param name="name">Optional name to search for (partial match).</param>
    /// <param name="statusType">Optional status type to filter by (Active = 1, Inactive = 2, Merged = 3).</param>
    /// <param name="isActive">Optional flag to filter by active status.</param>
    /// <param name="pageNumber">The page number (1-based). Defaults to 1.</param>
    /// <param name="pageSize">The page size. Defaults to 10.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search results with pagination information.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(SearchPersonsQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchPersonsQueryResponse>> Search(
        [FromQuery] string? name = null,
        [FromQuery] int? statusType = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchPersonsQuery
        {
            Name = name,
            StatusType = statusType,
            IsActive = isActive,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <param name="command">The create person command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created person's alternate key.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePersonCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreatePersonCommandResponse>> Create(
        [FromBody] CreatePersonCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { personKey = response.PersonKey },
            response);
    }

    /// <summary>
    /// Imports a person from an external system with a specific alternate key.
    /// </summary>
    /// <param name="command">The import person command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The imported person's alternate key.</returns>
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportPersonCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportPersonCommandResponse>> Import(
        [FromBody] ImportPersonCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { personKey = response.PersonKey },
            response);
    }

    /// <summary>
    /// Updates a person's name.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The update person name command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPut("{personKey:guid}/name")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateName(
        Guid personKey,
        [FromBody] UpdatePersonNameCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates a person's gender.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The update person gender command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/gender")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGender(
        Guid personKey,
        [FromBody] UpdatePersonGenderCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates a person's birth date.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The update person birth date command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/birth-date")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBirthDate(
        Guid personKey,
        [FromBody] UpdatePersonBirthDateCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Changes a person's status.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The change person status command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeStatus(
        Guid personKey,
        [FromBody] ChangePersonStatusCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Activates a person (sets status to Active).
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The activate person command (optional notes).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(
        Guid personKey,
        [FromBody] ActivatePersonCommand? command = null,
        CancellationToken cancellationToken = default)
    {
        var activateCommand = command ?? new ActivatePersonCommand { PersonKey = personKey };
        var updateCommand = activateCommand with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deactivates a person (sets status to Inactive).
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The deactivate person command (optional notes).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        Guid personKey,
        [FromBody] DeactivatePersonCommand? command = null,
        CancellationToken cancellationToken = default)
    {
        var deactivateCommand = command ?? new DeactivatePersonCommand { PersonKey = personKey };
        var updateCommand = deactivateCommand with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Marks a person as merged (sets status to Merged).
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The mark person as merged command (optional notes).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{personKey:guid}/mark-merged")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsMerged(
        Guid personKey,
        [FromBody] MarkPersonAsMergedCommand? command = null,
        CancellationToken cancellationToken = default)
    {
        var mergeCommand = command ?? new MarkPersonAsMergedCommand { PersonKey = personKey };
        var updateCommand = mergeCommand with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Adds an external system identifier to a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="command">The add person identifier command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPost("{personKey:guid}/identifiers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddIdentifier(
        Guid personKey,
        [FromBody] AddPersonIdentifierCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { PersonKey = personKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Removes an external system identifier from a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="identifierTypeCode">The identifier type code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpDelete("{personKey:guid}/identifiers/{identifierTypeCode:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveIdentifier(
        Guid personKey,
        int identifierTypeCode,
        CancellationToken cancellationToken)
    {
        var command = new RemovePersonIdentifierCommand
        {
            PersonKey = personKey,
            IdentifierTypeCode = identifierTypeCode
        };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
