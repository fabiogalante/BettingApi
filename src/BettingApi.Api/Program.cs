using BettingApi.Api.Endpoints;
using BettingApi.Api.Middlewares;
using BettingApi.Application;
using BettingApi.Application.Customers.Commands;
using BettingApi.Infrastructure;
using BettingApi.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediator(typeof(CreateCustomerCommandHandler).Assembly
);

builder.Services.AddInfrastructure();


builder.Host.UseSerilog((_, configuration) =>
    configuration
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] {Message:lj}{NewLine}{Exception}")
        .MinimumLevel.Information()
        .Enrich.FromLogContext());


var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature?.Error is not null)
        {
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = exceptionHandlerFeature.Error.Message
            });
        }
    });
});

app.UseStatusCodePages(async statusCodeContext =>
{
    statusCodeContext.HttpContext.Response.ContentType = "application/problem+json";
    
    await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
    {
        Status = statusCodeContext.HttpContext.Response.StatusCode,
        Title = "An error occurred",
        Detail = $"Status Code: {statusCodeContext.HttpContext.Response.StatusCode}"
    });
});

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados inicial");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCustomersEndpoints(); 

app.Run();

