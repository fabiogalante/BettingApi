// src/BettingApi.Application/Customers/Commands/UpdateCustomerCommand.cs

using System.Text.RegularExpressions;
using BettingApi.Application.Common;
using BettingApi.Application.Common.Interfaces;
using BettingApi.Core.Common;
using BettingApi.Core.Common.Interfaces;
using BettingApi.Core.Customers;
using ErrorOr;

namespace BettingApi.Application.Customers.Commands;

public record UpdateCustomerCommand(
    string Id,
    string Name,
    string Email) : ICommand<ErrorOr<Unit>>;



public class UpdateCustomerCommandHandler(
    IRepository<Customer> customerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCustomerCommand, ErrorOr<Unit>>
{
    private const string EmailRegexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public async Task<ErrorOr<Unit>> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        // Validar ID
        if (!Guid.TryParse(command.Id, out var customerId))
        {
            return Error.Validation(description: "ID do cliente inválido");
        }

        // Validar email
        if (!Regex.IsMatch(command.Email, EmailRegexPattern))
        {
            return Error.Validation(description: "Formato de email inválido");
        }

        // Buscar cliente
        var customer = await customerRepository.GetByIdAsync(EntityId.From(customerId), cancellationToken);
        if (customer is null)
        {
            return Error.NotFound(description: "Cliente não encontrado");
        }

        // Verificar se email já existe (para outro cliente)
        var emailExists = await customerRepository.FindAsync(
            c => c.Email == command.Email && c.Id.Value != customerId,
            cancellationToken);

        if (emailExists.Any())
        {
            return Error.Conflict(description: "Email já está em uso por outro cliente");
        }

        // Atualizar
        customer.Name = command.Name;
        customer.Email = command.Email;

        customerRepository.Update(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}