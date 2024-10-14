using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Domain.Interfaces;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Application.Services.Common;

public class TypeService : Service<Type>, ITypeService
{
    private readonly ITypeRepository _typeRepository;

    public TypeService(ITypeRepository typeRepository) : base(typeRepository)
    {
        _typeRepository = typeRepository;
    }
}