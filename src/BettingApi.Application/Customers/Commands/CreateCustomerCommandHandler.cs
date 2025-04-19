// CreateCustomerCommandHandler.cs

using BettingApi.Application.Common;
using ErrorOr;
using BettingApi.Core.Customers;
using BettingApi.Core.Common.Interfaces;
using BettingApi.Application.Common.Interfaces;

namespace BettingApi.Application.Customers.Commands;

public class CreateCustomerCommandHandler(
    IRepository<Customer> customerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> HandleAsync(
        CreateCustomerCommand command, 
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = command.Name,
            Email = command.Email,
        };
        
        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id.Value;
    }
}