using API.Accounts.DTOs;
using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.CreateRole;
using Application.Accounts.Commands.LoginAccount;
using Application.Accounts.Commands.UpdateRole;
using AutoMapper;

namespace API.Profiles;

public class APIProfiles : Profile
{
    public APIProfiles()
    {
        CreateMap<LoginAccountRequest, LoginAccountCommand>();
        CreateMap<LoginAccountCommand, LoginAccountRequest>();

        CreateMap<CreateAccountRequest, CreateAccountCommand>();
        CreateMap<CreateAccountCommand, CreateAccountRequest>();

        CreateMap<CreateRoleRequest, CreateRoleCommand>();
        CreateMap<CreateRoleCommand, CreateRoleRequest>();

        CreateMap<UpdateRoleRequest, UpdateRoleCommand>();
        CreateMap<UpdateRoleCommand, UpdateRoleRequest>();
    }
}