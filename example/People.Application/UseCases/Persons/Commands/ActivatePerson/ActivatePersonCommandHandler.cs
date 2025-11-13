namespace People.Application.UseCases.Persons.Commands.ActivatePerson;

using Core.LibrariesApplication.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;

/// <summary>
/// Handler for the ActivatePersonCommand.
/// </summary>
public class ActivatePersonCommandHandler : ICommandHandler<ActivatePersonCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="ActivatePersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public ActivatePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the ActivatePersonCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    public async Task Handle(
        ActivatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        person.Activate(request.Notes ?? "Person activated");

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

