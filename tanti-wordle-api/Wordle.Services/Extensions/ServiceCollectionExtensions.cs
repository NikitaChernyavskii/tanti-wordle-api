using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Wordle.Services.Contracts.Words;
using Wordle.Services.Words;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddServicesServices(this IServiceCollection services)
    {
        services.AddScoped<IWordsService, WordsService>();

        services.AddValidators();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IWordsServiceValidator, WordsServiceValidator>();
        
    }
}
