using Microsoft.Extensions.DependencyInjection;
using Wordle.Repository.Contracts.Words;
using Wordle.Repository.Words;

namespace Wordle.Repository.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddServicesServices(this IServiceCollection services)
    {
        services.AddScoped<IWordsRepository, WordsRepository>();
    }
}
