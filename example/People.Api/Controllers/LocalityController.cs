using MediatR;
using Microsoft.AspNetCore.Mvc;
using People.Application.UseCases.Localities.Queries.GetLocalityByKey;
using People.Application.UseCases.Localities.Queries.GetLocalitiesByType;
using People.Application.UseCases.Localities.Queries.SearchLocalities;
using People.Application.UseCases.Localities.Commands.CreateLocality;
using People.Application.UseCases.Localities.Commands.UpdateLocalityCache;
using People.Application.UseCases.Localities.Commands.ImportLocality;
using People.Application.UseCases.Localities.Commands.DeleteLocality;

namespace People.Api.Controllers;

/// <summary>
/// Controller for managing locality cache entries.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LocalityController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of <see cref="LocalityController"/>.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public LocalityController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets a locality by its alternate key.
    /// </summary>
    /// <param name="localityKey">The locality's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The locality reference information, or 404 if not found.</returns>
    [HttpGet("{localityKey:guid}")]
    [ProducesResponseType(typeof(GetLocalityByKeyQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetLocalityByKeyQueryResponse>> GetByKey(
        Guid localityKey,
        CancellationToken cancellationToken)
    {
        var query = new GetLocalityByKeyQuery { LocalityKey = localityKey };
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Gets all localities of a specific type.
    /// </summary>
    /// <param name="localityTypeCode">The locality type code (City = 1, Country = 2, State = 3, Neighborhood = 4, Street = 5).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of locality references.</returns>
    [HttpGet("type/{localityTypeCode:int}")]
    [ProducesResponseType(typeof(GetLocalitiesByTypeQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetLocalitiesByTypeQueryResponse>> GetByType(
        int localityTypeCode,
        CancellationToken cancellationToken)
    {
        var query = new GetLocalitiesByTypeQuery { LocalityTypeCode = localityTypeCode };
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Searches localities by name and type with pagination.
    /// </summary>
    /// <param name="name">Optional locality name to search for (partial match).</param>
    /// <param name="localityTypeCode">Optional locality type code to filter by.</param>
    /// <param name="pageNumber">The page number (1-based). Defaults to 1.</param>
    /// <param name="pageSize">The page size. Defaults to 10.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The search results with pagination information.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(SearchLocalitiesQueryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchLocalitiesQueryResponse>> Search(
        [FromQuery] string? name = null,
        [FromQuery] int? localityTypeCode = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchLocalitiesQuery
        {
            Name = name,
            LocalityTypeCode = localityTypeCode,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new locality cache entry.
    /// </summary>
    /// <param name="command">The create locality command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created locality's alternate key.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateLocalityCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateLocalityCommandResponse>> Create(
        [FromBody] CreateLocalityCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { localityKey = response.LocalityKey },
            response);
    }

    /// <summary>
    /// Imports a locality cache entry from an external system with a specific alternate key.
    /// </summary>
    /// <param name="command">The import locality command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The imported locality's alternate key.</returns>
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportLocalityCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportLocalityCommandResponse>> Import(
        [FromBody] ImportLocalityCommand command,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetByKey),
            new { localityKey = response.LocalityKey },
            response);
    }

    /// <summary>
    /// Updates a locality cache entry.
    /// </summary>
    /// <param name="localityKey">The locality's alternate key.</param>
    /// <param name="command">The update locality cache command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpPut("{localityKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCache(
        Guid localityKey,
        [FromBody] UpdateLocalityCacheCommand command,
        CancellationToken cancellationToken)
    {
        var updateCommand = command with { LocalityKey = localityKey };
        await _mediator.Send(updateCommand, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes a locality cache entry.
    /// </summary>
    /// <param name="localityKey">The locality's alternate key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if successful, or 404 if not found.</returns>
    [HttpDelete("{localityKey:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid localityKey,
        CancellationToken cancellationToken)
    {
        var command = new DeleteLocalityCommand { LocalityKey = localityKey };
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
