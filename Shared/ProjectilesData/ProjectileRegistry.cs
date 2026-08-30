
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
                
                Id = ProjectileType.Default, BaseSpeed = 10.0f, Height = 0.5f, Radius = 0.25f, ScenePath = ""

            }

        };

        _projectiles = list.ToFrozenDictionary(b => b.Id);


    }

    public static bool TryGetProjectile(ProjectileType id, out ProjectileData ability)
        {
        return _projectiles.TryGetValue(id, out ability);
        }

}