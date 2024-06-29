﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityProvider_API.Infrastructure.HealthChecks;

public class ApplicationHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(HealthCheckResult.Healthy("Healthy"));
    }
}