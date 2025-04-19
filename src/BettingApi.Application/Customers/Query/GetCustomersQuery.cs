using BettingApi.Application.Common.Interfaces;
using BettingApi.Core.Common;
using BettingApi.Core.Common.Interfaces;
using BettingApi.Core.Customers;

namespace BettingApi.Application.Customers.Query;

public record GetCustomerByIdQuery(Guid Id) : IQuery<string?>;

public class GetUserByIdQueryHandler(IRepository<Customer> customerRepository) : IQueryHandler<GetCustomerByIdQuery, string?>
{
    public async Task<string?> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var id = new EntityId(query.Id);
        var customers = await customerRepository.GetByIdAsync(id, cancellationToken);
        return customers?.Name;
    }
}
