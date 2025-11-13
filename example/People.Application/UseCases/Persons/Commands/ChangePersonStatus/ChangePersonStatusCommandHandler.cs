namespace People.Application.UseCases.Persons.Commands.ChangePersonStatus;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Persons.Entities.Statuses;
using People.Domain.Persons.Services.Repositories;

/// <summary>
/// Handler for the ChangePersonStatusCommand.
/// </summary>
public class ChangePersonStatusCommandHandler : ICommandHandler<ChangePersonStatusCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="ChangePersonStatusCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public ChangePersonStatusCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the ChangePersonStatusCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    /// <exception cref="ArgumentException">Thrown when the status type is invalid.</exception>
    public async Task Handle(
        ChangePersonStatusCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        if (!Enum.IsDefined(typeof(PersonStatusType), request.StatusType))
            throw new ArgumentException($"Invalid status type: {request.StatusType}", nameof(request));

        var statusType = (PersonStatusType)request.StatusType;
        person.ChangeStatus(statusType, request.Notes ?? string.Empty);

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

