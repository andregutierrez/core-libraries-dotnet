namespace People.Application.UseCases.Persons.Commands.AddPersonIdentifier;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;

/// <summary>
/// Handler for the AddPersonIdentifierCommand.
/// </summary>
public class AddPersonIdentifierCommandHandler : ICommandHandler<AddPersonIdentifierCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="AddPersonIdentifierCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public AddPersonIdentifierCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the AddPersonIdentifierCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    /// <exception cref="ArgumentException">Thrown when the identifier type is invalid.</exception>
    public async Task Handle(
        AddPersonIdentifierCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        var identifierType = IdentifierType.FromCode(request.IdentifierTypeCode);
        person.AddIdentifier(identifierType, request.ExternalId);

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

