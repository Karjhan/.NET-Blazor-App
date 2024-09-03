using Application.Requests.VehicleBrands;
using Application.Requests.VehicleOwners;
using Application.Requests.Vehicles;
using Application.Vehicles.Commands.CreateVehicle;
using Application.Vehicles.Commands.CreateVehicleBrand;
using Application.Vehicles.Commands.CreateVehicleOwner;
using Application.Vehicles.Commands.DeleteVehicle;
using Application.Vehicles.Commands.DeleteVehicleBrand;
using Application.Vehicles.Commands.DeleteVehicleOwner;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.Commands.UpdateVehicleBrand;
using Application.Vehicles.Commands.UpdateVehicleOwner;
using Application.Vehicles.Queries.GetSingleVehicle;
using Application.Vehicles.Queries.GetSingleVehicleBrand;
using Application.Vehicles.Queries.GetSingleVehicleOwner;
using Application.Vehicles.Queries.GetVehicleBrands;
using Application.Vehicles.Queries.GetVehicleOwners;
using Application.Vehicles.Queries.GetVehicles;
using Carter;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Vehicles;

public class VehiclesModule : CarterModule
{
    public VehiclesModule() : base(ApplicationConstants.ApiVehicleBasePath)
    {
        
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ApplicationConstants.ApiCreateVehicleSubPath, async ([FromBody] CreateVehicleRequest request, ISender sender) =>
        {
            CreateVehicleCommand command = request.ToCreateVehicleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicles");
        
        app.MapPost(ApplicationConstants.ApiCreateVehicleBrandSubPath, async ([FromBody] CreateVehicleBrandRequest request, ISender sender) =>
        {
            CreateVehicleBrandCommand command = request.ToCreateVehicleBrandCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleBrands");
        
        app.MapPost(ApplicationConstants.ApiCreateVehicleOwnerSubPath, async ([FromBody] CreateVehicleOwnerRequest request, ISender sender) =>
        {
            CreateVehicleOwnerCommand command = request.ToCreateVehicleOwnerCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleOwners");
        
        
        app.MapGet(ApplicationConstants.ApiGetVehiclesSubPath, async (ISender sender) =>
        {
            GetVehiclesQuery query = new GetVehiclesQuery();

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicles");
        
        app.MapGet(ApplicationConstants.ApiGetVehicleBrandsSubPath, async (ISender sender) =>
        {
            GetVehicleBrandsQuery query = new GetVehicleBrandsQuery();

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicleBrands");
        
        app.MapGet(ApplicationConstants.ApiGetVehicleOwnersSubPath, async (ISender sender) =>
        {
            GetVehicleOwnersQuery query = new GetVehicleOwnersQuery();

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicleOwners");
        
        
        app.MapGet("/get-vehicle/{vehicleId}", async (Guid vehicleId, ISender sender) =>
        {
            GetSingleVehicleQuery query = new GetSingleVehicleQuery(vehicleId);

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicles");
        
        app.MapGet("/get-vehicle-brand/{vehicleBrandId}", async (Guid vehicleBrandId, ISender sender) =>
        {
            GetSingleVehicleBrandQuery query = new GetSingleVehicleBrandQuery(vehicleBrandId);

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicleBrands");
        
        app.MapGet("/get-vehicle-owner/{vehicleOwnerId}", async (Guid vehicleOwnerId, ISender sender) =>
        {
            GetSingleVehicleOwnerQuery query = new GetSingleVehicleOwnerQuery(vehicleOwnerId);

            var result = await sender.Send(query);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        }).WithTags("vehicleOwners");
        
        
        app.MapDelete("/delete-vehicle/{vehicleId}", async (Guid vehicleId, ISender sender) =>
        {
            DeleteVehicleCommand command = new DeleteVehicleCommand(vehicleId);

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicles");
        
        app.MapDelete("/delete-vehicle-brand/{vehicleBrandId}", async (Guid vehicleBrandId, ISender sender) =>
        {
            DeleteVehicleBrandCommand command = new DeleteVehicleBrandCommand(vehicleBrandId);

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleBrands");
        
        app.MapDelete("/delete-vehicle-owner/{vehicleOwnerId}", async (Guid vehicleOwnerId, ISender sender) =>
        {
            DeleteVehicleOwnerCommand command = new DeleteVehicleOwnerCommand(vehicleOwnerId);

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleOwners");
        
        
        app.MapPut(ApplicationConstants.ApiUpdateVehicleSubPath, async ([FromBody] UpdateVehicleRequest request, ISender sender) =>
        {
            UpdateVehicleCommand command = request.ToUpdateVehicleCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicles");
        
        app.MapPut(ApplicationConstants.ApiUpdateVehicleBrandSubPath, async ([FromBody] UpdateVehicleBrandRequest request, ISender sender) =>
        {
            UpdateVehicleBrandCommand command = request.ToUpdateVehicleBrandCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleBrands");
        
        app.MapPut(ApplicationConstants.ApiUpdateVehicleOwnerSubPath, async ([FromBody] UpdateVehicleOwnerRequest request, ISender sender) =>
        {
            UpdateVehicleOwnerCommand command = request.ToUpdateVehicleOwnerCommand();

            var result = await sender.Send(command);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).WithTags("vehicleOwners");
    }
}