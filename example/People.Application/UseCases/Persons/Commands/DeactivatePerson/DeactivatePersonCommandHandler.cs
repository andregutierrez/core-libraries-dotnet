namespace People.Application.UseCases.Persons.Commands.DeactivatePerson;

using Core.Libraries.Domain.Entities.Identifiers;
using Core.Libraries.Domain.Exceptions;
using People.Domain.Persons.Services.Repositories;
using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Services.Repositories;
using Core.Libraries.Domain.Entities;

/// <summary>
/// Handler for the DeactivatePersonCommand.
/// </summary>
public class DeactivatePersonCommandHandler : ICommandHandler<DeactivatePersonCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="DeactivatePersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public DeactivatePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the DeactivatePersonCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    public async Task Handle(
        DeactivatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        person.Deactivate(request.Notes ?? "Person deactivated");

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

