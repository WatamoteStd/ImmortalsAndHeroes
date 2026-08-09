
using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase
{
    
    public int MaxHealth {get; protected set;}
    protected int _health;
    public int Health {
        
        get => _health;
        set => _health = Math.Clamp(value, 0, MaxHealth);
    
    }

    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        
        _health = (int)_data.BaseHealth;
        MaxHealth = (int)_data.BaseHealth;

    }

}