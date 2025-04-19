using BettingApi.Application.Common.Interfaces;

namespace BettingApi.Application.Customers.Commands;

public record CreateCustomerCommand(string Name, string Email) : ICommand<Guid>;