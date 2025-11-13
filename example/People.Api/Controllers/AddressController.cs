using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.UseCases.Addresses.Commands;
using People.Application.UseCases.Addresses.Commands.CreateAddress;
using People.Application.UseCases.Addresses.Commands.DeleteAddress;
using People.Application.UseCases.Addresses.Commands.ImportAddress;
using People.Application.UseCases.Addresses.Commands.RemoveAddressAsPrimary;
using People.Application.UseCases.Addresses.Commands.SetAddressAsPrimary;
using People.Application.UseCases.Addresses.Commands.UpdateAddress;
using People.Application.UseCases.Addresses.Commands.UpdateAddressNotes;
using People.Application.UseCases.Addresses.Commands.UpdateAddressType;
using People.Application.UseCases.Addresses.Queries.GetAddressByKey;
using People.Application.UseCases.Addresses.Queries.GetAddressesByPersonKey;
using People.Application.UseCases.Addresses.Queries.GetPrimaryAddressByPersonKey;
using People.Application.UseCases.Addresses.Queries.SearchAddresses;

namespace People.Api.Controllers;

/// <summary>
/// Controller for managing addresses.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of <see cref="AddressController"/>.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public AddressController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets an address by its alternate key.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed address information, or 404 if not found.</returns>
    [HttpGet("{addressKey:guid}")]
    [ProducesResponseType(typeof(GetAddressByKeyQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetAddressByKeyQueryResponse>> GetByKey(
        Guid addressKey,
        CancellationToken cancellationToken)
    {
        var query = new GetAddressByKeyQuery { AddressKey = addressKey };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Gets all addresses for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of detailed addresses.</returns>
    [HttpGet("person/{personKey:guid}")]
    [ProducesResponseType(typeof(GetAddressesByPersonKeyQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAddressesByPersonKeyQueryResponse>> GetByPersonKey(
        Guid personKey,
        CancellationToken cancellationToken)
    {
        var query = new GetAddressesByPersonKeyQuery { PersonKey = personKey };
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Gets the primary address for a person.
    /// </summary>
    /// <param name="personKey">The person's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detailed primary address, or 404 if not found.</returns>
    [HttpGet("person/{personKey:guid}/primary")]
    [ProducesResponseType(typeof(GetPrimaryAddressByPersonKeyQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPrimaryAddressByPersonKeyQueryResponse>> GetPrimaryByPersonKey(
        Guid personKey,
        CancellationToken cancellationToken)
    {
        var query = new GetPrimaryAddressByPersonKeyQuery { PersonKey = personKey };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Searches addresses with filters and pagination.
    /// </summary>
    /// <param name="personKey">Optional person key to filter by.</param>
    /// <param name="addressTypeCode">Optional address type code to filter by.</param>
    /// <param name="isPrimary">Optional flag to filter by primary status.</param>
    /// <param name="cityKey">Optional city key to filter by.</param>
    /// <param name="stateKey">Optional state key to filter by.</param>
    /// <param name="countryKey">Optional country key to filter by.</param>
    /// <param name="pageNumber">The page number (1-based). Defaults to 1.</param>
    /// <param name="pageSize">The page size. Defaults to 10.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search results with pagination information.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(SearchAddressesQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchAddressesQueryResponse>> Search(
        [FromQuery] Guid? personKey = null,
        [FromQuery] int? addressTypeCode = null,
        [FromQuery] bool? isPrimary = null,
        [FromQuery] Guid? cityKey = null,
        [FromQuery] Guid? stateKey = null,
        [FromQuery] Guid? countryKey = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchAddressesQuery
        {
            PersonKey = personKey,
            AddressTypeCode = addressTypeCode,
            IsPrimary = isPrimary,
            CityKey = cityKey,
            StateKey = stateKey,
            CountryKey = countryKey,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new address for a person.
    /// </summary>
    /// <param name="command">The create address command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created address's alternate key.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateAddressCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateAddressCommandResponse>> Create(
        [FromBody] CreateAddressCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { addressKey = response.AddressKey },
            response);
    }

    /// <summary>
    /// Imports an address from an external system with a specific alternate key.
    /// </summary>
    /// <param name="command">The import address command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The imported address's alternate key.</returns>
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportAddressCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportAddressCommandResponse>> Import(
        [FromBody] ImportAddressCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { addressKey = response.AddressKey },
            response);
    }

    /// <summary>
    /// Updates an existing address.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="command">The update address command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPut("{addressKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid addressKey,
        [FromBody] UpdateAddressCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { AddressKey = addressKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates the type of an address.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="command">The update address type command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{addressKey:guid}/type")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateType(
        Guid addressKey,
        [FromBody] UpdateAddressTypeCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { AddressKey = addressKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates the notes of an address.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="command">The update address notes command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{addressKey:guid}/notes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNotes(
        Guid addressKey,
        [FromBody] UpdateAddressNotesCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { AddressKey = addressKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Sets an address as the primary address for its person.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{addressKey:guid}/set-primary")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetAsPrimary(
        Guid addressKey,
        CancellationToken cancellationToken)
    {
        var command = new SetAddressAsPrimaryCommand { AddressKey = addressKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Removes the primary flag from an address.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPatch("{addressKey:guid}/remove-primary")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAsPrimary(
        Guid addressKey,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAddressAsPrimaryCommand { AddressKey = addressKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes an address.
    /// </summary>
    /// <param name="addressKey">The address's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpDelete("{addressKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid addressKey,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAddressCommand { AddressKey = addressKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
