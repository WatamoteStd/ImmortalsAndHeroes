

namespace Shared.Characters;

public static class EntityRegistry
{
    
    private static Dictionary<EntityType, EntityData> _entitiesRegistry = new Dictionary<EntityType, EntityData>()
    {
        {EntityType.WolfWeak, new EntityData
            {
            
                Height = 1f, Radius = 0.3f, ScenePath = "res://Entities/Mobs/Wolf/WeakWolf.tscn", BaseHealth = 80, Name = "WeakWolf",
                BaseDamage = 15, AttackRange = 1.1f, AttackSpeed = 100, Armor = 1, MagicResistance = 0, BaseSpeed = 3.5f,
                HealthRegeneration = 1.3f, BaseMana = 30, ManaRegeneration = 0.3f, BasicAttackTime = 1.15f,

                MaxExpReward = 28,
                MinExpReward = 20

            }
        },
        {EntityType.ForestBear, new EntityData
        {
            Height = 2.4f, Radius = 1.2f, ScenePath = "res://Entities/Mobs/Bears/ForestBear.tscn", BaseHealth = 345, Name = "Forest Bear",
            BaseDamage = 43, AttackRange = 1.3f, AttackSpeed = 100, Armor = 3, MagicResistance = 0, BaseSpeed = 2.4f,
            HealthRegeneration = 2.6f, BaseMana = 60, ManaRegeneration = 0.3f, BasicAttackTime = 2.7f,

            MaxExpReward = 155,
            MinExpReward = 125
        }
        },
        {EntityType.Default, new EntityData
            {
            
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Default/character_default.tscn", BaseHealth = 220, Name = "Default",
                BaseDamage = 22, AttackRange = 1.5f, AttackSpeed = 100, Armor = 0, MagicResistance = 0, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 90, ManaRegeneration = 0.85f, BasicAttackTime = 1.6f

            }
        },
        {EntityType.Male, new EntityData
            {
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Male/character_male.tscn", BaseHealth = 220, Name = "Male",
                BaseDamage = 22, AttackRange = 1.5f, AttackSpeed = 100, Armor = 0, MagicResistance = 0, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 90, ManaRegeneration = 0.85f, BasicAttackTime = 1.6f
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
    public required uint BaseHealth { get; init; }
    public required string Name { get; init; }
    public required uint BaseDamage { get; init; }
    public required float AttackRange { get; init; }
    public required int AttackSpeed { get; init; }
    public required int Armor { get; init; }
    public required int MagicResistance { get; init; }
    public required float BaseSpeed { get; init; }
    public required float HealthRegeneration { get; init; }
    public required uint BaseMana { get; init; }
    public required float ManaRegeneration { get; init; }
    public required float BasicAttackTime { get; init; }

    public uint? MinExpReward {get; init;}
    public uint? MaxExpReward {get; init;}

    }