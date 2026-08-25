

namespace Shared.Characters;

public static class EntityRegistry
{
    
    private static Dictionary<EntityType, EntityData> _entitiesRegistry = new Dictionary<EntityType, EntityData>()
    {
        {EntityType.WolfWeak, new EntityData
            {
            
                Height = 1f, Radius = 0.3f, ScenePath = "res://Entities/Mobs/Wolf/WeakWolf.tscn", BaseHealth = 80f, Name = "WeakWolf",
                BaseDamage = 15f, AttackRange = 1.1f, AttackSpeed = 100, Armor = 1f, MagicResistance = 0f, BaseSpeed = 3.5f,
                HealthRegeneration = 1.3f, BaseMana = 30f, ManaRegeneration = 0.3f, BasicAttackTime = 1.15f,

                MaxExpReward = 28,
                MinExpReward = 20

            }
        },
        {EntityType.ForestBear, new EntityData
        {
            Height = 2.4f, Radius = 1.2f, ScenePath = "res://Entities/Mobs/Bears/ForestBear.tscn", BaseHealth = 345f, Name = "Forest Bear",
            BaseDamage = 43f, AttackRange = 1.3f, AttackSpeed = 100f, Armor = 3f, MagicResistance = 0f, BaseSpeed = 2.4f,
            HealthRegeneration = 2.6f, BaseMana = 60f, ManaRegeneration = 0.3f, BasicAttackTime = 2.7f,

            MaxExpReward = 155,
            MinExpReward = 125
        }
        },
        {EntityType.Default, new EntityData
            {
            
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Default/character_default.tscn", BaseHealth = 220, Name = "Default",
                BaseDamage = 22f, AttackRange = 1.5f, AttackSpeed = 100, Armor = 0, MagicResistance = 0, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 100f, ManaRegeneration = 0.85f, BasicAttackTime = 1.6f

            }
        },
        {EntityType.Male, new EntityData
            {
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Male/character_male.tscn", BaseHealth = 220f, Name = "Male",
                BaseDamage = 22f, AttackRange = 1.5f, AttackSpeed = 100f, Armor = 0, MagicResistance = 0f, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 100f, ManaRegeneration = 0.85f, BasicAttackTime = 1.6f
            }
        }

    };
    private static EntityData _baseData = new EntityData
    {
        Height = 2f, Radius = 0.5f, ScenePath = "res://Models/Default.tscn", BaseHealth = 50, Name = "BaseEntity", BaseDamage = 1,
        AttackRange = 1f, AttackSpeed = 100, Armor = 1, MagicResistance = 1, BaseSpeed = 1f, HealthRegeneration = 1, BaseMana = 11, ManaRegeneration = 0.11f,
        BasicAttackTime = 5.0f
    };
    public static EntityData GetEntityData(EntityType entityType)
    {
        
        if (_entitiesRegistry.TryGetValue(entityType, out EntityData data))
            return data;
        else
        {
            Console.WriteLine($"[ENTITY REGISTRY] Invalid entityType:{entityType}. Return default result.");
            return _baseData;
        }

    }




}
public readonly struct EntityData
    {
        
    public required float Height { get; init; }
    public required float Radius { get; init; }
    public required string ScenePath { get; init; }
    public required float BaseHealth { get; init; }
    public required string Name { get; init; }
    public required float BaseDamage { get; init; }
    public required float AttackRange { get; init; }
    public required float AttackSpeed { get; init; }
    public required float Armor { get; init; }
    public required float MagicResistance { get; init; }
    public required float BaseSpeed { get; init; }
    public required float HealthRegeneration { get; init; }
    public required float BaseMana { get; init; }
    public required float ManaRegeneration { get; init; }
    public required float BasicAttackTime { get; init; }

    public uint? MinExpReward {get; init;}
    public uint? MaxExpReward {get; init;}

    }