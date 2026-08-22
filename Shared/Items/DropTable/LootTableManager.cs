
using Shared.Characters;

namespace Shared.Items.DropTable;

public static class LootTableManager
{
    
    private static readonly Dictionary<EntityType, LootDrop[]> _entityToDrop = new Dictionary<EntityType, LootDrop[]>
    {
        
        {EntityType.WolfWeak, new LootDrop[] {
            new LootDrop(ItemType.WolfSkin_Horrible, 0.25f, 1, 2),
            new LootDrop(ItemType.WolfSkin_Normal, 0.1f, 1, 2),
            new LootDrop(ItemType.WolfSkin_Primordial, 0.02f, 1, 1)
        }}

    };

    public static LootDrop[] GetEntityDropTable(EntityType entity)
    {
        
        if (_entityToDrop.TryGetValue(entity, out var lootDrops))
        {
            return lootDrops;
        }
        return Array.Empty<LootDrop>();

    }

}