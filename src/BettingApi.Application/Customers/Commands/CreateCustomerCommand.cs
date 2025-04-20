using BettingApi.Application.Common.Interfaces;
using ErrorOr;

namespace BettingApi.Application.Customers.Commands;

public record CreateCustomerCommand(string Name, string Email) : ICommand<ErrorOr<Guid>>;