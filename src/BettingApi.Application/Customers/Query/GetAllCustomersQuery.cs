using BettingApi.Application.Common.Interfaces;
using BettingApi.Core.Customers;
using BettingApi.Core.Common.Interfaces;

namespace BettingApi.Application.Customers.Query;

// Query record (representa a requisição).
public record GetAllCustomersQuery : IQuery<IEnumerable<Customer>>;

// Handler da query.
public class GetAllCustomersQueryHandler(IRepository<Customer> customerRepository)
    : IQueryHandler<GetAllCustomersQuery, IEnumerable<Customer>>
{
    public async Task<IEnumerable<Customer>> HandleAsync(GetAllCustomersQuery query, CancellationToken cancellationToken = default)
    {
        return await customerRepository.GetAllAsync(cancellationToken);
    }
}