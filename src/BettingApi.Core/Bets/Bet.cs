using BettingApi.Core.Common;
using BettingApi.Core.Customers;

namespace BettingApi.Core.Bets;

public class Bet : Entity
{
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public required EntityId CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}