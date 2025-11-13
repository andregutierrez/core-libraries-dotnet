namespace People.Application.UseCases.Persons.Commands.ImportPerson;

using People.Domain.Persons.Services.Repositories;
using People.Domain.Persons.ValueObjects;
using Core.Libraries.Application.Commands;
using Core.Libraries.Domain.Services.Repositories;
using Core.Libraries.Domain.Entities;
using People.Domain.Persons.Entities;

/// <summary>
/// Handler for the ImportPersonCommand.
/// </summary>
public class ImportPersonCommandHandler : ICommandHandler<ImportPersonCommand, ImportPersonCommandResponse>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="ImportPersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public ImportPersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the ImportPersonCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the imported person's alternate key.</returns>
    public async Task<ImportPersonCommandResponse> Handle(
        ImportPersonCommand request,
        CancellationToken cancellationToken)
    {
        var key = new AlternateKey(request.PersonKey);
        var name = new PersonName(request.FirstName, request.LastName, request.MiddleName, request.SocialName);
        BirthDate? birthDate = request.BirthDate.HasValue ? new BirthDate(request.BirthDate.Value) : null;
        Gender? gender = request.GenderCode.HasValue ? Gender.FromCode(request.GenderCode.Value) : null;

        var person = Person.Import(key, name, birthDate, gender);

        await _personRepository.InsertAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ImportPersonCommandResponse
        {
            PersonKey = person.Key.Value
        };
    }
}

