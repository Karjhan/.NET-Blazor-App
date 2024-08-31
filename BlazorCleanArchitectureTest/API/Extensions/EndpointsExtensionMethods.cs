using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace API.Extensions;

public static class EndpointsExtensionMethods
{
    public static IEndpointConventionBuilder MapApplicationHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        // All
        var result = endpoints.MapHealthChecks("/healthCheck", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return result;
    }
}