using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class EntityBase
{
    
    public uint EntityId {get; protected set;}
    public uint RegionId {get; set;}
    public string Name {get; protected set;}
    public Vector3 Position {get; protected set;}
    protected readonly EntityData _dllData;

    public bool IsAlive = true;

    public int MaxHealth {get; protected set;}
    protected int _health;
    public int Health {
        
    get => _health;
    set => _health = Math.Clamp(value, 0, MaxHealth);
    
    }
    public float HealthRegeneration {get; protected set;}
    private int _mana;
    public int MaxMana {get; protected set;}
    public int Mana
    {
        get => _mana;
        set => _mana = Math.Clamp(value, 0, MaxMana);
    }
    public float ManaRegeneration {get; protected set;}
    public float BaseSpeed {get; protected set;}

    public EntityType ModelType {get; protected set;}

    public float Radius => _dllData.Radius;
    public float Height => _dllData.Height;



    public EntityBase(uint entityId, Vector3 pos, EntityType type, uint regionId)
    {
        
        EntityId = entityId;
        Position = pos;
        ModelType = type;
        RegionId = regionId;
        _dllData = EntityRegistry.GetEntityData(type);
        Name =_dllData.Name;
        _health = (int)_dllData.BaseHealth;
        MaxHealth = (int)_dllData.BaseHealth;

        // BATTLE STATS
        BaseSpeed = _dllData.BaseSpeed;
        HealthRegeneration = _dllData.HealthRegeneration;
        _mana = (int)_dllData.BaseMana;
        MaxMana = (int)_dllData.BaseMana;
        ManaRegeneration = _dllData.ManaRegeneration;


    }

    public virtual void Update(float deltaTime)
    {
        


    }

    public virtual void SetPosition(float x, float y, float z)
    {
        Position = new Vector3(x,y,z);
    }


}