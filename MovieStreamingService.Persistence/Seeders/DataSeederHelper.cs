using MovieStreamingService.Domain.Interfaces;
using MovieStreamingService.Persistence.Seeders.DataGenerators;

namespace MovieStreamingService.Persistence.Seeders;

public class DataSeederHelper
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IFavouriteRepository _favouriteRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IStudioRepository _studioRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;

    public DataSeederHelper(
        ICommentRepository commentRepository, 
        ICountryRepository countryRepository, 
        IEpisodeRepository episodeRepository, 
        IFavouriteRepository favouriteRepository, 
        IMovieRepository movieRepository, 
        IPersonRepository personRepository, 
        IReviewRepository reviewRepository, 
        ISeasonRepository seasonRepository, 
        IStatusRepository statusRepository, 
        IStudioRepository studioRepository, 
        ISubscriptionRepository subscriptionRepository, 
        ITagRepository tagRepository, 
        ITypeRepository typeRepository, 
        IUserRepository userRepository, 
        IUserSubscriptionRepository userSubscriptionRepository
        )
    {
        _commentRepository = commentRepository;
        _countryRepository = countryRepository;
        _episodeRepository = episodeRepository;
        _favouriteRepository = favouriteRepository;
        _movieRepository = movieRepository;
        _personRepository = personRepository;
        _reviewRepository = reviewRepository;
        _seasonRepository = seasonRepository;
        _statusRepository = statusRepository;
        _studioRepository = studioRepository;
        _subscriptionRepository = subscriptionRepository;
        _tagRepository = tagRepository;
        _typeRepository = typeRepository;
        _userRepository = userRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
    }

    public async Task SeedDataAsync()
    {
        await SeedCountriesAsync();
        await SeedPeopleAsync();
        await SeedStatusesAsync();
        await SeedStudiosAsync();
        await SeedSubscriptionsAsync();
        await SeedTagsAsync();
        await SeedTypesAsync();
        await SeedUserAsync();
        await SeedUserSubscriptionsAsync();
        await SeedMoviesAsync();
        await SeedEpisodesAsync();
        await SeedCommentsAsync();
        await SeedFavouritesAsync();
        await SeedReviewsAsync();
        await SeedSeasonsAsync();
    }

    private async Task SeedCommentsAsync()
    {
        var episodes = (await _episodeRepository.GetAllAsync()).ToList();
        var userIds = (await _userRepository.GetAllAsync()).Select(u => u.Id).ToList();
        var comments = CommentDataGenerator.GenerateComments(10, userIds, episodes, null);
        foreach (var comment in comments)
        {
            await _commentRepository.AddAsync(comment);
        }
    }

    private async Task SeedCountriesAsync()
    {
        var countries = CountryDataGenerator.GenerateCountries(10);
        foreach (var country in countries)
        {
            await _countryRepository.AddAsync(country);
        }
    }

    private async Task SeedEpisodesAsync()
    {
        var existingMovies = (await _movieRepository.GetAllAsync()).ToList();
        var episodes = EpisodeDataGenerator.GenerateEpisodes(10, existingMovies);
        foreach (var episode in episodes)
        {
            await _episodeRepository.AddAsync(episode);
        }
    }

    private async Task SeedFavouritesAsync()
    {
        var movies = (await _movieRepository.GetAllAsync()).ToList();
        var userIds = (await _userRepository.GetAllAsync()).Select(u => u.Id).ToList();
        var favourites = FavouriteDataGenerator.GenerateFavourites(10, userIds, movies);
        foreach (var favourite in favourites)
        {
            await _favouriteRepository.AddAsync(favourite);
        }
    }

    private async Task SeedMoviesAsync()
    {
        var types = (await _typeRepository.GetAllAsync()).ToList();
        var statuses = (await _statusRepository.GetAllAsync()).ToList();
        var movies = MovieDataGenerator.GenerateMovies(10, types, statuses);
        foreach (var movie in movies)
        {
            await _movieRepository.AddAsync(movie);
        }
    }

    private async Task SeedPeopleAsync()
    {
        var people = PersonDataGenerator.GeneratePeople(10);
        foreach (var person in people)
        {
            await _personRepository.AddAsync(person);
        }
    }

    private async Task SeedReviewsAsync()
    {
        var movies = (await _movieRepository.GetAllAsync()).ToList();
        var userIds = (await _userRepository.GetAllAsync()).Select(u => u.Id).ToList();
        var reviews = ReviewDataGenerator.GenerateReviews(10, userIds, movies);
        foreach (var review in reviews)
        {
            await _reviewRepository.AddAsync(review);
        }
    }

    private async Task SeedSeasonsAsync()
    {
        var existingMovies = (await _movieRepository.GetAllAsync()).ToList();
        var seasons = SeasonDataGenerator.GenerateSeasons(10, existingMovies);
        foreach (var season in seasons)
        {
            try
            {
                await _seasonRepository.AddAsync(season);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private async Task SeedStatusesAsync()
    {
        var statuses = StatusDataGenerator.GenerateStatuses(10);
        foreach (var status in statuses)
        {
            await _statusRepository.AddAsync(status);
        }
    }

    private async Task SeedStudiosAsync()
    {
        var studios = StudioDataGenerator.GenerateStudios(10);
        foreach (var studio in studios)
        {
            await _studioRepository.AddAsync(studio);
        }
    }

    private async Task SeedSubscriptionsAsync()
    {
        var subscriptions = SubscriptionDataGenerator.GenerateSubscriptions(10);
        foreach (var subscription in subscriptions)
        {
            await _subscriptionRepository.AddAsync(subscription);
        }
    }

    private async Task SeedTagsAsync()
    {
        var parentTags = (await _tagRepository.GetAllAsync()).ToList();
        var tags = TagDataGenerator.GenerateTags(10, parentTags);
        foreach (var tag in tags)
        {
            await _tagRepository.AddAsync(tag);
        }
    }

    private async Task SeedTypesAsync()
    {
        var types = TypeDataGenerator.GenerateTypes(10);
        foreach (var type in types)
        {
            await _typeRepository.AddAsync(type);
        }
    }

    private async Task SeedUserAsync()
    {
        var userRoles = UserDataGenerator.GenerateUsers(10);
        foreach (var userRole in userRoles)
        {
            await _userRepository.AddAsync(userRole);
        }
    }

    private async Task SeedUserSubscriptionsAsync()
    {
        var users = (await _userRepository.GetAllAsync()).Select(u => u.Id).ToList();
        var subscriptions = (await _subscriptionRepository.GetAllAsync()).ToList();
        var userSubscriptions = UserSubscriptionDataGenerator.GenerateUserSubscriptions(10, users, subscriptions);
        foreach (var userSubscription in userSubscriptions)
        {
            await _userSubscriptionRepository.AddAsync(userSubscription);
        }
    }
}