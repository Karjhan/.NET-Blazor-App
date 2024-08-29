using API.Extensions;
using API.Middlewares;
using Application;
using Application.Extensions;
using Carter;
using Infrastructure.Extensions;
using MediatR.NotificationPublishers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add logging with Serilog
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddSwaggerDocumentation();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Configuration.GetValue("EnforceHttpsRedirection", true))
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<RequestLogContextMiddleware>();

app.UseSerilogRequestLogging();

app.MapApplicationHealthChecks();

app.UseInfrastructureSwagger();

app.MapCarter();

app.Run();