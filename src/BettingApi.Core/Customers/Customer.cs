using BettingApi.Core.Bets;
using BettingApi.Core.Common;

namespace BettingApi.Core.Customers;

public class Customer : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<Bet> Bets { get; set; } = new List<Bet>();
}