
using System.Collections.Frozen;

namespace Shared.ProjectilesData;

public static class ProjectileRegistry
{
    
    private static FrozenDictionary<ProjectileType, ProjectileData> _projectiles = FrozenDictionary<ProjectileType, ProjectileData>.Empty;

    static ProjectileRegistry()
    {
        

        var list = new ProjectileData[]
        {
            
            new ProjectileData
            {
                Id = ProjectileType.Default, BaseSpeed = 10.0f, Height = 0.5f, Radius = 0.25f, ScenePath = "res://Entities/Projectiles/Prefabs/Defaulth/DefaulthProjectile.tscn"
            },
            new ProjectileData
            {
                Id = ProjectileType.UnknownMage, BaseSpeed = 12f, Height = 0.3f, Radius = 0.10f, ScenePath = "res://Entities/Projectiles/Prefabs/Mobs/UnknownMageProjectile.tscn"
            }

        };

        _projectiles = list.ToFrozenDictionary(b => b.Id);


    }

    public static bool TryGetProjectile(ProjectileType id, out ProjectileData ability)
        {
        return _projectiles.TryGetValue(id, out ability);
        }

}