
namespace Shared.ProjectilesData;

public readonly struct ProjectileData
{
    
    public required ProjectileType Id {get; init;}
    public required float Height {get; init;}
    public required float Radius {get; init;}
    public required string ScenePath {get; init;}
    public required float BaseSpeed {get; init;}

}