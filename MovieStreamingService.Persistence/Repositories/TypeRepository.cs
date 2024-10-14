using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Persistence.Context;
using Type = MovieStreamingService.Domain.Models.Type;

namespace MovieStreamingService.Persistence.Repositories;

public class TypeRepository : Repository<Type>, ITypeRepository
{
    public TypeRepository(AppDbContext context) : base(context)
    {
    }
}