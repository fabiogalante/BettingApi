// src/BettingApi.Application/Customers/Commands/DeleteCustomerCommand.cs

using System.Windows.Input;
using BettingApi.Application.Common;
using BettingApi.Application.Common.Interfaces;
using BettingApi.Core.Bets;
using BettingApi.Core.Common;
using BettingApi.Core.Common.Interfaces;
using BettingApi.Core.Customers;
using ErrorOr;

namespace BettingApi.Application.Customers.Commands;

public record DeleteCustomerCommand(string Id) : ICommand<ErrorOr<Unit>>;


public class DeleteCustomerCommandHandler(
    IRepository<Customer> customerRepository,
    IRepository<Bet> betRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCustomerCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(command.Id, out var customerId))
        {
            return Error.Validation(description: "ID do cliente inválido");
        }

        var customer = await customerRepository.GetByIdAsync(EntityId.From(customerId), cancellationToken);
        if (customer is null)
        {
            return Error.NotFound(description: "Cliente não encontrado");
        }

        // Verificar se há apostas associadas
        var hasBets = await betRepository.FindAsync(
            b => b.CustomerId.Value == customerId,
            cancellationToken);

        if (hasBets.Any())
        {
            return Error.Conflict(description: "Não é possível deletar cliente com apostas associadas");
        }

        customerRepository.Remove(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}