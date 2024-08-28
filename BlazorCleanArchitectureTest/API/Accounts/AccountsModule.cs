using API.Accounts.DTOs;
using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.CreateAdmin;
using Application.Accounts.Commands.CreateRole;
using Application.Accounts.Commands.UpdateRole;
using Application.Accounts.Queries.GetAccountsWithRoles;
using Application.Accounts.Queries.GetRoles;
using Application.Accounts.Queries.LoginAccount;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts;

public class AccountsModule : CarterModule
{
    public AccountsModule() : base("api/accounts")
    {
        
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/create", async ([FromBody] CreateAccountRequest request, ISender sender) =>
        {
            CreateAccountCommand command = request.ToCreateAccountCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("accounts");
        
        app.MapPost("/identity/login", async ([FromBody] LoginAccountRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            LoginAccountQuery query = request.ToLoginAccountQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("accounts");
        
        app.MapPost("/identity/role/create", async ([FromBody] CreateRoleRequest request, ISender sender) =>
        {
            CreateRoleCommand command = request.ToCreateRoleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("roles");
        
        app.MapGet("/identity/role/list", async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetRolesQuery query = new GetRolesQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("roles");
        
        app.MapPost("/setting", async (ISender sender) =>
        {
            CreateAdminCommand command = new CreateAdminCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("accounts");
        
        app.MapPost("/identity/users-with-roles", async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetAccountsWithRolesQuery query = new GetAccountsWithRolesQuery();

            var response = await sender.Send(query, cancellationToken);
            
            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
        }).WithTags("accounts");
        
        app.MapGet("/identity/change-role", async ([FromBody] UpdateRoleRequest request, ISender sender) =>
        {
            UpdateRoleCommand command = request.ToUpdateRoleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("roles");
    }
}