using Application.Utilities;
using Carter;
using Infrastructure.Configurations;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.ExternalAuth;

public class ExternalAuthModule : CarterModule
{
    public ExternalAuthModule() : base(ApplicationConstants.ApiExternalAuthPath)
    {

    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/exchange-token/{externalJwtToken}", (string externalJwtToken, IOptions<JWTConfiguration> jwtConfiguration) =>
        {
            var result = AccountUtilities.ExchangeExternalAuthToken(externalJwtToken, jwtConfiguration.Value);
            
            return Task.FromResult(!string.IsNullOrEmpty(result) ? Results.Ok(result) : Results.BadRequest());
        }).WithTags("external-auth");
    }
}
    
