namespace People.Application.UseCases.Persons.Commands.CreatePerson;

using Core.LibrariesApplication.Commands;
using Core.LibrariesDomain.Services.Repositories;
using People.Domain.Persons.Services.Repositories;
using People.Domain.Persons.ValueObjects;

/// <summary>
/// Handler for the CreatePersonCommand.
/// </summary>
public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, CreatePersonCommandResponse>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of <see cref="CreatePersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository">The person repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public CreatePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Handles the CreatePersonCommand.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the created person's alternate key.</returns>
    public async Task<CreatePersonCommandResponse> Handle(
        CreatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var name = new PersonName(request.FirstName, request.LastName, request.MiddleName, request.SocialName);
        BirthDate? birthDate = request.BirthDate.HasValue ? new BirthDate(request.BirthDate.Value) : null;
        Gender? gender = request.GenderCode.HasValue ? Gender.FromCode(request.GenderCode.Value) : null;

        var person = Person.Create(name, birthDate, gender);

        await _personRepository.InsertAsync(person, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreatePersonCommandResponse
        {
            PersonKey = person.Key.Value
        };
    }
}

