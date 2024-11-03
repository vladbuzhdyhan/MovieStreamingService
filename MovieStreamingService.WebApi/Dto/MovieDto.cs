using MovieStreamingService.Domain.Enums;

namespace MovieStreamingService.WebApi.Dto;
public record MovieDto(int Id,
    string Name,
    string Description,
    string RestrictedRating,
    string Poster,
    int? Duration,
    DateTime? FirstAirDate,
    DateTime? LastAirDate,
    int? AmountOfEpisodes,
    decimal ImdbRating,
    string Background
    );
