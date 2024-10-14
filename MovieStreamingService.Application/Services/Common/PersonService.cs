using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Application.Services.Common;

public class PersonService : Service<Person>, IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository) : base(personRepository)
    {
        _personRepository = personRepository;
    }
}