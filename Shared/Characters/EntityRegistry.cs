

namespace Shared.Characters;

public static class EntityRegistry
{
    
    private static Dictionary<EntityType, EntityData> _entitiesRegistry = new Dictionary<EntityType, EntityData>()
    {
        {EntityType.WolfWeak, new EntityData
            {
            
                Height = 1f, Radius = 0.3f, ScenePath = "res://Entities/Mobs/Wolf/WeakWolf.tscn", BaseHealth = 80, Name = "WeakWolf",
                BaseDamage = 15, AttackRange = 1.1f, AttackSpeed = 100, Armor = 1, MagicResistance = 0, BaseSpeed = 3.2f,
                HealthRegeneration = 1.3f, BaseMana = 30, ManaRegeneration = 0.3f

            }
        },
        {EntityType.Default, new EntityData
            {
            
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Default/character_default.tscn", BaseHealth = 220, Name = "Default",
                BaseDamage = 22, AttackRange = 1.5f, AttackSpeed = 100, Armor = 0, MagicResistance = 0, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 90, ManaRegeneration = 0.85f

            }
        },
        {EntityType.Male, new EntityData
            {
                Height = 2f, Radius = 0.5f, ScenePath = "res://Entities/Character/Male/character_male.tscn", BaseHealth = 220, Name = "Male",
                BaseDamage = 22, AttackRange = 1.5f, AttackSpeed = 100, Armor = 0, MagicResistance = 0, BaseSpeed = 3f,
                HealthRegeneration = 2f, BaseMana = 90, ManaRegeneration = 0.85f
            }
        }

    };
    private static EntityData _baseData = new EntityData
    {
        Height = 2f, Radius = 0.5f, ScenePath = "res://Models/Default.tscn", BaseHealth = 50, Name = "BaseEntity", BaseDamage = 1,
        AttackRange = 1f, AttackSpeed = 100, Armor = 1, MagicResistance = 1, BaseSpeed = 1f, HealthRegeneration = 1, BaseMana = 11, ManaRegeneration = 0.11f
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
        
    public float Height { get; init; }
    public float Radius { get; init; }
    public string ScenePath { get; init; }
    public uint BaseHealth { get; init; }
    public string Name { get; init; }
    public uint BaseDamage { get; init; }
    public float AttackRange { get; init; }
    public int AttackSpeed { get; init; }
    public int Armor { get; init; }
    public int MagicResistance { get; init; }
    public float BaseSpeed { get; init; }
    public float HealthRegeneration { get; init; }
    public uint BaseMana { get; init; }
    public float ManaRegeneration { get; init; }


    }