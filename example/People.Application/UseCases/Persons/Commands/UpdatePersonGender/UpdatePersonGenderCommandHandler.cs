namespace People.Application.UseCases.Persons.Commands.UpdatePersonGender;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities.Identifiers;
using Core.LibrariesDomain.Exceptions;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Handler for the UpdatePersonGenderCommand.
/// </summary>
public class UpdatePersonGenderCommandHandler : ICommandHandler<UpdatePersonGenderCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdatePersonGenderCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdatePersonGenderCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the UpdatePersonGenderCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    public async Task Handle(
        UpdatePersonGenderCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        Gender? gender = request.GenderCode.HasValue ? Gender.FromCode(request.GenderCode.Value) : null;
        person.UpdateGender(gender);

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

