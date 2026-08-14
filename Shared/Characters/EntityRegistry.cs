

namespace Shared.Characters;

public static class EntityRegistry
{
    
    private static Dictionary<EntityType, EntityData> _entitiesRegistry = new Dictionary<EntityType, EntityData>()
    {
        
        {EntityType.WolfWeak, new EntityData(1f, 0.3f, "res://Entities/Mobs/Wolf/WeakWolf.tscn", 80, "WeakWolf")},
        {EntityType.Default, new EntityData(2f, 0.5f, "res://Entities/Character/Default/character_default.tscn", 220, "Default")},
        {EntityType.Male, new EntityData(2f, 0.5f,"res://Entities/Character/Male/character_male.tscn", 220, "Male")}

    };
    private static EntityData _baseData = new EntityData(2f, 0.5f,"res://Models/Default.tscn", 50, "BaseEntity");

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
        
        public readonly float Height;
        public readonly float Radius;
        public readonly string ScenePath;
        public readonly uint BaseHealth;
        public readonly string Name;

        public EntityData(float height, float radius, string path, uint baseHealth, string name)
        {
            
            Height = height;
            Radius = radius;
            ScenePath = path;
            BaseHealth = baseHealth;
            Name = name;
            
        }

    }