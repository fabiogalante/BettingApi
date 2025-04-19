using BettingApi.Application.Common;
using BettingApi.Application.Common.Interfaces;
using BettingApi.Application.Customers.Commands;
using BettingApi.Application.Customers.Query;
using Microsoft.AspNetCore.Mvc;

namespace BettingApi.Api.Endpoints;

public static class CustomersEndpoints
{
    public static void MapCustomersEndpoints(this WebApplication app)
    {
        app.MapPost("/api/customers", async ([FromBody] CreateCustomerCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.SendCommandAsync<CreateCustomerCommand, Guid>(command);
            return Results.Created($"/api/customers", result);
        });
        
        app.MapGet("/api/customers/{id:guid}", async ([FromRoute] Guid id,[FromServices] IMediator mediator) =>
        {
            var query = new GetCustomerByIdQuery(id);
            var result = await mediator.SendQueryAsync<GetCustomerByIdQuery, string?>(query);
            return Results.Ok(result);
        });
    }
}