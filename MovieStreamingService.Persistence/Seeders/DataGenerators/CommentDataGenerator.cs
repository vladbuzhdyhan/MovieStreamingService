using Bogus;
using MovieStreamingService.Domain.Models;

namespace MovieStreamingService.Persistence.Seeders.DataGenerators;

public class CommentDataGenerator
{
    public static List<Comment> GenerateComments(int count, List<Guid> userIds, List<Episode> episodes, List<Comment>? parentComments)
    {
        var episodeIds = episodes.Select(e => e.Id).ToList();

        return new Faker<Comment>()
            .RuleFor(c => c.Text, f => f.Lorem.Sentence())
            .RuleFor(c => c.EpisodeId, f => f.PickRandom(episodeIds))
            .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
            .RuleFor(c => c.ParentId, f => parentComments != null && parentComments.Count > 0 ? f.PickRandom(parentComments).Id : null)
            .RuleFor(c => c.CreatedAt, f => DateTime.Now)
            .RuleFor(c => c.UpdatedAt, f => DateTime.Now)
            .Generate(count);
    }
}