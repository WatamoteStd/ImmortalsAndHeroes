

namespace Shared.Characters;

public static class EntityRegistry
{
    
    private static Dictionary<EntityType, EntityData> _entitiesRegistry = new Dictionary<EntityType, EntityData>()
    {
        
        {EntityType.WolfWeak, new EntityData(1f, 0.4f, "res://Models/Mobs/Wolf/WeakWolf.tscn", 80)}

    };
    private static EntityData _baseData = new EntityData(1.5f, 0.5f,"res://Models/Default.tscn", 50);

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



    public readonly struct EntityData
    {
        
        public readonly float Height;
        public readonly float Radius;
        public readonly string ScenePath;
        public readonly uint BaseHealth;

        public EntityData(float height, float radius, string path, uint baseHealth)
        {
            
            Height = height;
            Radius = radius;
            ScenePath = path;
            BaseHealth = baseHealth;
            
        }

    }

}