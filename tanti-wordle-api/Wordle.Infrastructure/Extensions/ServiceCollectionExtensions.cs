using Microsoft.Extensions.DependencyInjection;
using Wordle.Services.Extensions;

namespace Wordle.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWordleServices(this IServiceCollection services)
        {
            services.AddServicesServices();
        }
    }
}
