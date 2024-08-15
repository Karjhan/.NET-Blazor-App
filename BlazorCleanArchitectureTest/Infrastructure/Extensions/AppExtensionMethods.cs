using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Extensions;

public static class AppExtensionMethods
{
    public static WebApplication UseInfrastructureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors();

        return app;
    }
}