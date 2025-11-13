namespace People.Application.UseCases.Persons.Commands.MarkPersonAsMerged;

using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Entities;
using Core.Libraries.Domain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;

/// <summary>
/// Handler for the MarkPersonAsMergedCommand.
/// </summary>
public class MarkPersonAsMergedCommandHandler : ICommandHandler<MarkPersonAsMergedCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="MarkPersonAsMergedCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public MarkPersonAsMergedCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the MarkPersonAsMergedCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the person is not found.</exception>
    public async Task Handle(
        MarkPersonAsMergedCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetAsync(
            new AlternateKey(request.PersonKey),
            cancellationToken);

        person.MarkAsMerged(request.Notes ?? "Person merged");

        await _personRepository.UpdateAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

