namespace People.Application.UseCases.Persons.Commands.UpdatePersonBirthDate;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Handler for the UpdatePersonBirthDateCommand.
/// </summary>
public class UpdatePersonBirthDateCommandHandler : ICommandHandler<UpdatePersonBirthDateCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdatePersonBirthDateCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdatePersonBirthDateCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdatePersonBirthDateCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    public async Task Handle(
        UpdatePersonBirthDateCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        BirthDate? birthDate = request.BirthDate.HasValue ? new BirthDate(request.BirthDate.Value) : null;
        person.UpdateBirthDate(birthDate);

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

