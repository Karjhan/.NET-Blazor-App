using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.CreateAdmin;
using Application.Accounts.Commands.CreateRole;
using Application.Accounts.Commands.UpdateRole;
using Application.Accounts.Queries.GetAccountsWithRoles;
using Application.Accounts.Queries.GetRoles;
using Application.Accounts.Queries.LoginAccount;
using Application.Accounts.Queries.RefreshToken;
using Application.Requests.Accounts;
using Carter;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts;

public class AccountsModule : CarterModule
{
    public AccountsModule() : base(ApplicationConstants.ApiAccountBasePath)
    {
        
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ApplicationConstants.ApiCreateAccountSubPath, async ([FromBody] CreateAccountRequest request, ISender sender) =>
        {
            CreateAccountCommand command = request.ToCreateAccountCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("accounts");
        
        app.MapPost(ApplicationConstants.ApiLoginAccountSubPath, async ([FromBody] LoginAccountRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            LoginAccountQuery query = request.ToLoginAccountQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("accounts");
        
        app.MapPost(ApplicationConstants.ApiRefreshTokenSubPath, async ([FromBody] RefreshTokenRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            RefreshTokenQuery query = request.ToRefreshTokenQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("accounts");
        
        app.MapPost(ApplicationConstants.ApiCreateRoleSubPath, async ([FromBody] CreateRoleRequest request, ISender sender) =>
        {
            CreateRoleCommand command = request.ToCreateRoleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("roles");
        
        app.MapGet(ApplicationConstants.ApiGetRolesSubPath, async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetRolesQuery query = new GetRolesQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("roles");
        
        app.MapPost(ApplicationConstants.ApiCreateAdminSubPath, async (ISender sender) =>
        {
            CreateAdminCommand command = new CreateAdminCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("accounts");
        
        app.MapGet(ApplicationConstants.ApiGetAccountsWithRolesSubPath, async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetAccountsWithRolesQuery query = new GetAccountsWithRolesQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("accounts");
        
        app.MapPost(ApplicationConstants.ApiUpdateRoleSubPath, async ([FromBody] UpdateRoleRequest request, ISender sender) =>
        {
            UpdateRoleCommand command = request.ToUpdateRoleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("roles");
    }
}