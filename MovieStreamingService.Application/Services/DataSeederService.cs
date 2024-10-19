using MovieStreamingService.Persistence.Seeders;

namespace MovieStreamingService.Application.Services;
public class DataSeederService
{
    private readonly DataSeederHelper dataSeederHelper;
    public DataSeederService(DataSeederHelper dataSeederHelper)
    {
        this.dataSeederHelper = dataSeederHelper;
    }

    public async Task SeedDataAsync()
    {
        await dataSeederHelper.SeedDataAsync();
    }
}
