using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.UseCases.Contacts.Commands;
using People.Application.UseCases.Contacts.Queries;

namespace People.Api.Controllers;

/// <summary>
/// Controller for managing phone contacts (Phone, Mobile, WhatsApp).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ContactPhoneController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of <see cref="ContactPhoneController"/>.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public ContactPhoneController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets a phone contact by its alternate key.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed contact information, or 404 if not found.</returns>
    [HttpGet("{contactKey:guid}")]
    [ProducesResponseType(typeof(GetContactByKeyQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetContactByKeyQueryResponse>> GetByKey(
        Guid contactKey,
        CancellationToken cancellationToken)
    {
        var query = new GetContactByKeyQuery { ContactKey = contactKey };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Gets all phone contacts for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="contactTypeId">The contact type ID (Phone = 2, Mobile = 3, WhatsApp = 4).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of phone contacts.</returns>
    [HttpGet("person/{personKey:guid}")]
    [ProducesResponseType(typeof(GetContactsByPersonKeyAndTypeQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetContactsByPersonKeyAndTypeQueryResponse>> GetByPersonKey(
        Guid personKey,
        [FromQuery] int contactTypeId,
        CancellationToken cancellationToken)
    {
        var query = new GetContactsByPersonKeyAndTypeQuery
        {
            PersonKey = personKey,
            ContactTypeId = contactTypeId
        };
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Gets the primary phone contact of a specific type for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="contactTypeId">The contact type ID (Phone = 2, Mobile = 3, WhatsApp = 4).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The primary phone contact, or 404 if not found.</returns>
    [HttpGet("person/{personKey:guid}/primary")]
    [ProducesResponseType(typeof(GetPrimaryContactByPersonKeyAndTypeQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPrimaryContactByPersonKeyAndTypeQueryResponse>> GetPrimaryByPersonKey(
        Guid personKey,
        [FromQuery] int contactTypeId,
        CancellationToken cancellationToken)
    {
        var query = new GetPrimaryContactByPersonKeyAndTypeQuery
        {
            PersonKey = personKey,
            ContactTypeId = contactTypeId
        };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Creates a new phone contact for a person.
    /// </summary>
    /// <param name="command">The create phone contact command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created contact's alternate key.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePhoneContactCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreatePhoneContactCommandResponse>> Create(
        [FromBody] CreatePhoneContactCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { contactKey = response.ContactKey },
            response);
    }

    /// <summary>
    /// Imports a phone contact from an external system with a specific alternate key.
    /// </summary>
    /// <param name="command">The import contact command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The imported contact's alternate key.</returns>
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportContactCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportContactCommandResponse>> Import(
        [FromBody] ImportContactCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { contactKey = response.ContactKey },
            response);
    }

    /// <summary>
    /// Updates a phone contact's phone number.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="command">The update phone contact command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPut("{contactKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid contactKey,
        [FromBody] UpdatePhoneContactCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { ContactKey = contactKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates the notes of a phone contact.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="command">The update contact notes command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{contactKey:guid}/notes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNotes(
        Guid contactKey,
        [FromBody] UpdateContactNotesCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { ContactKey = contactKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Sets a phone contact as the primary phone contact of its type for its person.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{contactKey:guid}/set-primary")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetAsPrimary(
        Guid contactKey,
        CancellationToken cancellationToken)
    {
        var command = new SetContactAsPrimaryCommand { ContactKey = contactKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Removes the primary flag from a phone contact.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{contactKey:guid}/remove-primary")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAsPrimary(
        Guid contactKey,
        CancellationToken cancellationToken)
    {
        var command = new RemoveContactAsPrimaryCommand { ContactKey = contactKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes a phone contact.
    /// </summary>
    /// <param name="contactKey">The contact's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpDelete("{contactKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid contactKey,
        CancellationToken cancellationToken)
    {
        var command = new DeleteContactCommand { ContactKey = contactKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
