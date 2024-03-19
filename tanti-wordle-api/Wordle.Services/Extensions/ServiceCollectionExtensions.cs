using Microsoft.Extensions.DependencyInjection;
using Wordle.Services.Contracts.Words;
using Wordle.Services.Words;

namespace Wordle.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesServices(this IServiceCollection services)
    {
        services.AddScoped<IWordsService, WordsService>();
    }
}
