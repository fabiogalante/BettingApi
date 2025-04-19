namespace BettingApi.Core.Common;

public abstract class Entity
{
    public EntityId Id { get; protected set; } = EntityId.New();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}