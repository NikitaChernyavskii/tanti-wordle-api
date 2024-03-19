using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Wordle.Api.Infrastructure;

[ExcludeFromCodeCoverage]
public class HealthCheckWithDependencies : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;
        StringBuilder unhealthyDependecies = new StringBuilder();

        // check dependencies here: SQL connections, apis, etc.

        if (!isHealthy)
        {
            var dependencies = unhealthyDependecies.ToString();
            return Task.FromResult(HealthCheckResult.Unhealthy($"Unhealthy dependencies: {dependencies}."));
        }

        return Task.FromResult(HealthCheckResult.Healthy("All dependencies are healthy."));
    }
}
