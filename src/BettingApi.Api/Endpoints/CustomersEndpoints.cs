using BettingApi.Application.Common.Interfaces;
using BettingApi.Application.Customers.Commands;
using BettingApi.Application.Customers.Query;
using BettingApi.Core.Customers;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace BettingApi.Api.Endpoints;

public static class CustomersEndpoints
{
    public static void MapCustomersEndpoints(this WebApplication app)
    {
        app.MapPost("/api/customers", async ([FromBody] CreateCustomerCommand command, IMediator mediator) =>
        {
            var result = await mediator.SendCommandAsync<CreateCustomerCommand, ErrorOr<Guid>>(command);
            return Results.Created($"/api/customers", result);
        });
        
        app.MapGet("/api/customers/{id:guid}", async ([FromRoute] Guid id,IMediator mediator) =>
        {
            var query = new GetCustomerByIdQuery(id);
            var result = await mediator.SendQueryAsync<GetCustomerByIdQuery, string?>(query);
            return Results.Ok(result);
        });
       
        
        app.MapGet("/api/customers", async (IMediator mediator) =>
        {
            var query = new GetAllCustomersQuery();
            var result = await mediator.SendQueryAsync<GetAllCustomersQuery, IEnumerable<Customer>>(query);
            return Results.Ok(result);
        });

    }
}